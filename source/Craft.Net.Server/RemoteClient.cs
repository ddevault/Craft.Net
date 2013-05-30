using Craft.Net.Anvil;
using Craft.Net.Common;
using Craft.Net.Entities;
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
    public class RemoteClient : INotifyPropertyChanged
    {
        public RemoteClient(TcpClient client)
        {
            NetworkClient = client;
            PacketQueue = new ConcurrentQueue<IPacket>();
            KnownEntities = new List<int>();
            LoadedChunks = new List<Coordinates2D>();
            Settings = new ClientSettings();
        }

        public TcpClient NetworkClient { get; set; }
        public MinecraftStream NetworkStream { get; set; }
        public bool IsLoggedIn { get; set; }
        public ConcurrentQueue<IPacket> PacketQueue { get; set; }
        public ClientSettings Settings { get; set; }
        public bool EncryptionEnabled { get; protected internal set; }
        public string Hostname { get; set; }
        public short Ping { get; set; }
        public string Username { get; set; }
        public PlayerEntity Entity { get; set; }
        public List<Coordinates2D> LoadedChunks { get; set; }
        public DateTime LastKeepAlive { get; set; }
        public DateTime LastKeepAliveSent { get; set; }

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
        internal string AuthenticationHash { get; set; }

        protected internal byte[] SharedKey { get; set; }

        public void SendPacket(IPacket packet)
        {
            PacketQueue.Enqueue(packet);
        }

        public void Disconnect(string reason)
        {
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
                    SendPacket(new SpawnPlayerPacket(player.EntityId, player.Username, MathHelper.CreateAbsoluteInt(player.Position.X),
                        MathHelper.CreateAbsoluteInt(player.Position.Y), MathHelper.CreateAbsoluteInt(player.Position.Z),
                        MathHelper.CreateRotationByte(player.Yaw), MathHelper.CreateRotationByte(player.Pitch), 0, player.Metadata));
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

        /// <summary>
        /// Loads the given chunk on the client.
        /// </summary>
        public virtual void LoadChunk(Coordinates2D position)
        {
            var chunk = Entity.World.GetChunk(position);
            SendPacket(ChunkHelper.CreatePacket(chunk));
            // TODO: Tile entities
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
        public virtual void SendChat(string message)
        {
            SendPacket(new ChatMessagePacket(message));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected internal virtual void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
