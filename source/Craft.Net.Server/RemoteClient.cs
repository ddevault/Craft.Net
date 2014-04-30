using Craft.Net.Anvil;
using Craft.Net.Common;
using Craft.Net.Logic;
using Craft.Net.Networking;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Server
{
    public class RemoteClient : INotifyPropertyChanged, IDisposable
    {
        public RemoteClient(TcpClient client)
        {
            NetworkClient = client;
            PacketQueue = new ConcurrentQueue<IPacket>();
            KnownEntities = new List<int>();
            LoadedChunks = new List<Coordinates2D>();
            Settings = new ClientSettings();
            MaxDigDistance = 6;
            Tags = new Dictionary<string, object>();
        }

        public NetworkManager NetworkManager { get; set; }
        public TcpClient NetworkClient { get; set; }
        public Stream NetworkStream { get; set; }
        public bool IsLoggedIn { get; internal set; }
        public ConcurrentQueue<IPacket> PacketQueue { get; set; }
        public ClientSettings Settings { get; set; }
        public bool EncryptionEnabled { get; protected internal set; }
        public string Hostname { get; internal set; }
        public short Ping { get; internal set; }
        public string Username { get; internal set; }
        public PlayerEntity Entity { get; set; }
        public int Reach { get { return GameMode == GameMode.Creative ? 6 : 5; } } // TODO: Allow customization
        public int MaxDigDistance { get; set; }
        public Dictionary<string, object> Tags { get; set; }
        public string UUID { get; internal set; }

        protected internal List<Coordinates2D> LoadedChunks { get; set; }
        protected internal DateTime LastKeepAlive { get; set; }
        protected internal DateTime LastKeepAliveSent { get; set; }
        protected internal DateTime ExpectedMiningEnd { get; set; }
        protected internal Coordinates3D ExpectedBlockToMine { get; set; }
        protected internal int BlockBreakStageTime { get; set; }
        protected internal DateTime? BlockBreakStartTime { get; set; }
        protected internal byte[] VerificationToken { get; set; }
        protected internal List<short> PaintedSlots { get; set; }

        internal bool PauseChunkUpdates = false;
        internal PlayerManager PlayerManager { get; set; }

        public World World
        {
            get
            {
                return Entity.World;
            }
        }

        protected GameMode _GameMode;
        public GameMode GameMode
        {
            get { return _GameMode; }
            set
            {
                _GameMode = value;
                OnPropertyChanged("GameMode");
            }
        }

        internal List<int> KnownEntities { get; set; }
        internal string ServerId { get; set; }

        protected internal byte[] SharedKey { get; set; }

        public void SendPacket(IPacket packet)
        {
            PacketQueue.Enqueue(packet);
        }

        public void Disconnect(string reason)
        {
            if (NetworkManager.NetworkMode == NetworkMode.Login)
                SendPacket(new LoginDisconnectPacket(reason));
            else
                SendPacket(new DisconnectPacket(reason));
        }

        internal void TrackEntity(Entity entity)
        {
            if (!KnownEntities.Contains(entity.EntityId))
            {
                KnownEntities.Add(entity.EntityId);
                if (entity is PlayerEntity)
                {
                    var player = entity as PlayerEntity;
                    var selectedItem = player.SelectedItem.Id;
                    if (selectedItem == -1) selectedItem = 0;
                    SendPacket(new SpawnPlayerPacket(player.EntityId, UUID, player.Username, MathHelper.CreateAbsoluteInt(player.Position.X),
                        MathHelper.CreateAbsoluteInt(player.Position.Y), MathHelper.CreateAbsoluteInt(player.Position.Z),
                        MathHelper.CreateRotationByte(player.Yaw), MathHelper.CreateRotationByte(player.Pitch), selectedItem, player.Metadata));
                    if (!player.SelectedItem.Empty)
                        SendPacket(new EntityEquipmentPacket(entity.EntityId, EntityEquipmentPacket.EntityEquipmentSlot.HeldItem, player.SelectedItem));
                    // TODO: Send armor
                }
                else if (entity is ObjectEntity)
                {
                    var objectEntity = entity as ObjectEntity;
                    SendPacket(new SpawnObjectPacket(objectEntity.EntityId, objectEntity.EntityType, MathHelper.CreateAbsoluteInt(objectEntity.Position.X),
                        MathHelper.CreateAbsoluteInt(objectEntity.Position.Y), MathHelper.CreateAbsoluteInt(objectEntity.Position.Z), MathHelper.CreateRotationByte(objectEntity.Yaw),
                        MathHelper.CreateRotationByte(objectEntity.Pitch), objectEntity.Data, 0, 0, 0)); // TODO: Velocity stuff here
                    if (objectEntity.SendMetadataToClients)
                        SendPacket(new EntityMetadataPacket(objectEntity.EntityId, objectEntity.Metadata));
                }
            }
        }

        internal void ForgetEntity(Entity entity)
        {
            if (KnownEntities.Contains(entity.EntityId))
                KnownEntities.Remove(entity.EntityId);
            SendPacket(new DestroyEntityPacket(new int[] { entity.EntityId }));
        }

        internal void UpdateEntity(Entity entity)
        {
            // TODO: Check queue for existing updates for this entity and discard them
            SendPacket(new EntityTeleportPacket(entity.EntityId, MathHelper.CreateAbsoluteInt(entity.Position.X),
                MathHelper.CreateAbsoluteInt(entity.Position.Y), MathHelper.CreateAbsoluteInt(entity.Position.Z),
                MathHelper.CreateRotationByte(entity.Yaw), MathHelper.CreateRotationByte(entity.Pitch)));
            if (entity is LivingEntity)
                SendPacket(new EntityHeadLookPacket(entity.EntityId, MathHelper.CreateRotationByte((entity as LivingEntity).HeadYaw)));
        }

        /// <summary>
        /// Asyncronously updates chunks loaded on the client
        /// </summary>
        /// <returns></returns>
        public Task UpdateChunksAsync()
        {
            if ((int)(Entity.Position.X) >> 4 != (int)(Entity.OldPosition.X) >> 4 ||
                (int)(Entity.Position.Z) >> 4 != (int)(Entity.OldPosition.Z) >> 4)
            {
                return Task.Factory.StartNew(() => UpdateChunks(true));
            }
            return null;
        }

        /// <summary>
        /// Asyncronously updates chunks loaded on the client and forces a
        /// recalculation of which chunks should be loaded.
        /// </summary>
        public Task ForceUpdateChunksAsync()
        {
            return Task.Factory.StartNew(() => UpdateChunks(true));
        }

        /// <summary>
        /// Updates which chunks are loaded on the client.
        /// </summary>
        public virtual void UpdateChunks(bool forceUpdate)
        {
            if (!forceUpdate && PauseChunkUpdates)
                return;
            if (forceUpdate ||
                (int)(Entity.Position.X) >> 4 != (int)(Entity.OldPosition.X) >> 4 ||
                (int)(Entity.Position.Z) >> 4 != (int)(Entity.OldPosition.Z) >> 4)
            {
                var newChunks = new List<Coordinates2D>();
                for (int x = -Settings.ViewDistance; x < Settings.ViewDistance; x++)
                    for (int z = -Settings.ViewDistance; z < Settings.ViewDistance; z++)
                    {
                        newChunks.Add(new Coordinates2D(
                            ((int)Entity.Position.X >> 4) + x,
                            ((int)Entity.Position.Z >> 4) + z));
                    }
                // Unload extraneous columns
                lock (LoadedChunks)
                {
                    var currentChunks = new List<Coordinates2D>(LoadedChunks);
                    foreach (Coordinates2D chunk in currentChunks)
                    {
                        if (!newChunks.Contains(chunk))
                            UnloadChunk(chunk);
                    }
                    // Load new columns
                    foreach (Coordinates2D chunk in newChunks)
                    {
                        if (!LoadedChunks.Contains(chunk))
                            LoadChunk(chunk);
                    }
                }
            }
        }

        public virtual void UnloadAllChunks()
        {
            lock (LoadedChunks)
            {
                while (LoadedChunks.Any())
                {
                    UnloadChunk(LoadedChunks[0]);
                }
            }
        }

        /// <summary>
        /// Loads the given chunk on the client.
        /// </summary>
        public virtual void LoadChunk(Coordinates2D position)
        {
            var chunk = Entity.World.GetChunk(position);
            SendPacket(ChunkHelper.CreatePacket(chunk));
            // TODO: Tile entities
            foreach (var entity in chunk.TileEntities)
            {
                // ...
            }
            LoadedChunks.Add(position);
        }

        /// <summary>
        /// Unloads the given chunk on the client.
        /// </summary>
        public virtual void UnloadChunk(Coordinates2D position)
        {
            var dataPacket = new ChunkDataPacket();
            dataPacket.AddBitMap = 0;
            dataPacket.GroundUpContinuous = true;
            dataPacket.PrimaryBitMap = 0;
            dataPacket.X = (int)position.X;
            dataPacket.Z = (int)position.Z;
            dataPacket.Data = ChunkHelper.ChunkRemovalSequence;
            SendPacket(dataPacket);
            LoadedChunks.Remove(position);
        }

        /// <summary>
        /// Sends a <see cref="ChatMessagePacket"/> to the client.
        /// </summary>
        public virtual void SendChat(ChatMessage message)
        {
            //SendPacket(new ChatMessagePacket(string.Format("{{\"text\":\"{0}\"}}", text)));
            SendPacket(new ChatMessagePacket(message.ToJson()));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected internal virtual void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public void Dispose()
        {
            if (PlayerManager != null)
                PlayerManager.Dispose();
        }
    }
}
