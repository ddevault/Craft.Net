using Craft.Net.Anvil;
using Craft.Net.Common;
using Craft.Net.Entities;
using Craft.Net.Networking;
using Craft.Net.Server.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Craft.Net.Logic;
using Craft.Net.Physics;
using Craft.Net.Logic.Blocks;

namespace Craft.Net.Server
{
    public class MinecraftServer
    {
        public delegate void PacketHandler(RemoteClient client, MinecraftServer server, IPacket packet);

        #region Constructors

        public MinecraftServer()
        {
            LogicHelpers.Register();
            PacketHandlers = new PacketHandler[256];
            Handlers.PacketHandlers.RegisterHandlers(this);
            NetworkLock = new object();
            Clients = new List<RemoteClient>();
            Settings = ServerSettings.DefaultSettings;
            EntityManager = new EntityManager(this);
            LastTimeUpdate = DateTime.MinValue;
            NextChunkUpdate = DateTime.MinValue;
            PhysicsEngines = new List<PhysicsEngine>();
        }

        public MinecraftServer(Level level) : this()
        {
            Level = level;
        }

        public MinecraftServer(ServerSettings settings) : this()
        {
            Settings = settings;
        }

        public MinecraftServer(Level level, ServerSettings settings) : this(level)
        {
            Settings = settings;
        }

        #endregion

        #region Properties

        public List<RemoteClient> Clients { get; set; }
        public Level Level { get; set; }
        public TcpListener Listener { get; set; }
        public DateTime StartTime { get; private set; }
        public ServerSettings Settings { get; set; }
        public EntityManager EntityManager { get; set; }

        protected internal RSACryptoServiceProvider CryptoServiceProvider { get; set; }
        protected internal RSAParameters ServerKey { get; set; }
        protected internal object NetworkLock { get; set; }
        protected internal List<PhysicsEngine> PhysicsEngines { get; set; }

        protected Thread NetworkThread { get; set; }
        protected Thread EntityThread { get; set; }
        protected PacketHandler[] PacketHandlers { get; set; }

        private DateTime NextPlayerUpdate { get; set; }
        private DateTime NextChunkUpdate { get; set; }
        private DateTime LastTimeUpdate { get; set; }

        #endregion

        #region Public methods

        public void RegisterPacketHandler(byte packetId, PacketHandler handler)
        {
            PacketHandlers[packetId] = handler;
        }

        public void Start(IPEndPoint endPoint)
        {
            if (Level == null)
                throw new InvalidOperationException("Unable to start server without a level");

            foreach (var world in Level.Worlds)
            {
                world.BlockChange -= WorldBlockChange;
                world.BlockChange += WorldBlockChange;
                world.SpawnEntityRequested -= WorldSpawnEntityRequested;
                world.SpawnEntityRequested += WorldSpawnEntityRequested;
                PhysicsEngines.Add(new PhysicsEngine(world));
            }

            CryptoServiceProvider = new RSACryptoServiceProvider(1024);
            ServerKey = CryptoServiceProvider.ExportParameters(true);

            StartTime = DateTime.Now;

            Listener = new TcpListener(endPoint);
            Listener.Start();
            Listener.BeginAcceptTcpClient(AcceptClientAsync, null);

            NetworkThread = new Thread(NetworkWorker);
            EntityThread = new Thread(PhysicsWorker);
            NetworkThread.Start();
            EntityThread.Start();
        }

        public void Stop()
        {
            lock (NetworkLock)
            {
                if (Listener != null)
                {
                    Listener.Stop();
                    Listener = null;
                }
                if (NetworkThread != null)
                {
                    NetworkThread.Abort();
                    NetworkThread = null;
                }
                if (EntityThread != null)
                {
                    EntityThread.Abort();
                    EntityThread = null;
                }
            }
        }

        public void SendChat(string text)
        {
            text = string.Format("{{\"text\":\"{0}\"}}", text); // TODO: Implement this properly
            foreach (var client in Clients.Where(c => c.IsLoggedIn))
                client.SendChat(text);
        }

        public RemoteClient[] GetClientsInWorld(World world)
        {
            return Clients.Where(c => c.IsLoggedIn && c.World == world).ToArray();
        }

        public RemoteClient GetClient(string name)
        {
            return Clients.SingleOrDefault(c => c.Username == name);
        }

        public void DisconnectPlayer(RemoteClient client, string reason = null)
        {
            if (!Clients.Contains(client))
                throw new InvalidOperationException("The server is not aware of this client.");
            lock (NetworkLock)
            {
                if (reason != null)
                {
                    try
                    {
                        if (client.NetworkClient != null && client.NetworkClient.Connected)
                        {
                            new DisconnectPacket(reason).WritePacket(client.NetworkStream);
                            client.NetworkStream.Flush();
                        }
                    }
                    catch { }
                }
                try
                {
                    if (client.NetworkClient != null && client.NetworkClient.Connected)
                    {
                        client.NetworkClient.Close();
                    }
                }
                catch { }
                if (client.IsLoggedIn)
                    EntityManager.Despawn(client.Entity);
                Clients.Remove(client);
                var args = new PlayerLogInEventArgs(client);
                OnPlayerLoggedOut(args);
                if (!args.Handled)
                    SendChat(string.Format(ChatColors.Yellow + "{0} left the game.", client.Username));
            }
        }

        #endregion

        #region Protected methods

        protected void AcceptClientAsync(IAsyncResult result)
        {
            lock (NetworkLock)
            {
                if (Listener == null)
                    return; // Server shutting down
                var client = new RemoteClient(Listener.EndAcceptTcpClient(result));
				client.NetworkStream = new MinecraftStream(new BufferedStream(client.NetworkClient.GetStream()));
            	Clients.Add(client);
                Listener.BeginAcceptTcpClient(AcceptClientAsync, null);
            }
        }

        protected internal void UpdatePlayerList()
        {
            if (Clients.Count != 0)
            {
                for (int i = 0; i < Clients.Count; i++)
                {
                    foreach (RemoteClient client in Clients)
                    {
                        if (client.IsLoggedIn)
                            Clients[i].SendPacket(new PlayerListItemPacket(client.Username, true, client.Ping));
                    }
                }
            }
        }

        protected internal PhysicsEngine GetPhysicsForWorld(World world)
        {
            return PhysicsEngines.FirstOrDefault(p => p.World == world);
        }

        #endregion

        #region Internal methods

        internal void LogInPlayer(RemoteClient client)
        {
            // Spawn player
            // TODO: Load player data
            //client.Entity = Level.LoadPlayer(client.Username);
            client.Entity = new PlayerEntity(client.Username);
            // Temporary
            client.Entity.Position = Level.Spawn;
            client.Entity.SpawnPoint = Level.Spawn;
            client.GameMode = Level.GameMode;
            client.PlayerManager = new PlayerManager(client, this);
            EntityManager.SpawnEntity(Level.DefaultWorld, client.Entity);
            client.SendPacket(new LoginRequestPacket(client.Entity.EntityId,
                Level.DefaultWorld.WorldGenerator.GeneratorName, client.GameMode,
                Dimension.Overworld, Settings.Difficulty, Settings.MaxPlayers));
            client.SendPacket(new SpawnPositionPacket((int)client.Entity.SpawnPoint.X, (int)client.Entity.SpawnPoint.Y, (int)client.Entity.SpawnPoint.Z));
            client.SendPacket(new PlayerAbilitiesPacket(client.Entity.Abilities.AsFlags(), client.Entity.Abilities.FlyingSpeed, client.Entity.Abilities.WalkingSpeed));
            client.SendPacket(new EntityPropertiesPacket(client.Entity.EntityId,
                new[] { new EntityProperty("generic.movementSpeed", client.Entity.Abilities.WalkingSpeed) }));
            client.SendPacket(new TimeUpdatePacket(Level.Time, Level.Time));
            client.SendPacket(new SetWindowItemsPacket(0, client.Entity.Inventory.GetSlots()));

            // Send initial chunks
            client.UpdateChunks(true);
            // Adding 0.1 here prevents the client from falling through the ground upon logging in
            // Presumably, Minecraft runs some physics stuff and if it spawns exactly at ground level, it falls a little and
            // clips through the ground. This fixes that.
            client.SendPacket(new PlayerPositionAndLookPacket(client.Entity.Position.X, client.Entity.Position.Y + 0.1 + PlayerEntity.Height,
                client.Entity.Position.Z, client.Entity.Position.Y + 0.1, client.Entity.Yaw, client.Entity.Pitch, false));
            client.IsLoggedIn = true;
            UpdatePlayerList();

            // Send entities
            EntityManager.SendClientEntities(client);

            client.SendPacket(new UpdateHealthPacket(client.Entity.Health, client.Entity.Food, client.Entity.FoodSaturation));
            var args = new PlayerLogInEventArgs(client);
            OnPlayerLoggedIn(args);
            //LogProvider.Log(client.Username + " joined the game.");
            if (!args.Handled)
                SendChat(ChatColors.Yellow + client.Username + " joined the game.");
        }

        #endregion

        #region Private methods

        private void PhysicsWorker()
        {
            while (true)
            {
                foreach (var engine in PhysicsEngines)
                    engine.Update();
                EntityManager.Update();
                Thread.Sleep(PhysicsEngine.MillisecondsBetweenUpdates);
            }
        }

        private void NetworkWorker()
        {
            while (true)
            {
                UpdateScheduledEvents();
                lock (NetworkLock)
                {
                    for (int i = 0; i < Clients.Count; i++)
                    {
                        var client = Clients[i];
                        bool disconnect = false;
                        while (client.PacketQueue.Count != 0)
                        {
                            IPacket nextPacket;
                            if (client.PacketQueue.TryDequeue(out nextPacket))
                            {
                                nextPacket.WritePacket(client.NetworkStream);
                                client.NetworkStream.Flush();
                                if (nextPacket is DisconnectPacket)
                                    disconnect = true;
                                if (nextPacket is EncryptionKeyResponsePacket)
                                {
                                    client.NetworkStream = new MinecraftStream(new BufferedStream(
                                        new AesStream(client.NetworkClient.GetStream(), client.SharedKey)));
                                    client.EncryptionEnabled = true;
                                }
                            }
                        }
                        if (disconnect)
                        {
                            DisconnectPlayer(client);
                            i--;
                            continue;
                        }
                        // Read packets
                        var timeout = DateTime.Now.AddMilliseconds(10);
                        while (client.NetworkClient.Available != 0 && DateTime.Now < timeout)
                        {
                            try
                            {
                                var packet = PacketReader.ReadPacket(client.NetworkStream);
                                if (packet is DisconnectPacket)
                                {
                                    DisconnectPlayer(client);
                                    i--;
                                    break;
                                }
                                HandlePacket(client, packet);
                            }
                            catch (SocketException)
                            {
                                DisconnectPlayer(client);
                                i--;
                                break;
                            }
                            catch (Exception e)
                            {
                                DisconnectPlayer(client, e.Message);
                                i--;
                                break;
                            }
                        }
                        if (client.IsLoggedIn)
                            DoClientUpdates(client);
                    }
                }
                if (LastTimeUpdate != DateTime.MinValue)
                {
                    if ((DateTime.Now - LastTimeUpdate).TotalMilliseconds >= 50)
                    {
                        Level.Time += (long)((DateTime.Now - LastTimeUpdate).TotalMilliseconds / 50);
                        LastTimeUpdate = DateTime.Now;
                    }
                }
                if (NextChunkUpdate < DateTime.Now)
                    NextChunkUpdate = DateTime.Now.AddSeconds(1);
                Thread.Sleep(10);
            }
        }

        private void HandlePacket(RemoteClient client, IPacket packet)
        {
            if (PacketHandlers[packet.Id] == null)
                return;
                //throw new InvalidOperationException("No packet handler registered for 0x" + packet.Id.ToString("X2"));
            PacketHandlers[packet.Id](client, this, packet);
        }

        private void UpdateScheduledEvents()
        {
            if (DateTime.Now > NextPlayerUpdate)
            {
                UpdatePlayerList();
                NextPlayerUpdate = DateTime.Now.AddMinutes(1);
            }
        }

        private void DoClientUpdates(RemoteClient client)
        {
            // Update keep alive, chunks, etc
            if (client.LastKeepAliveSent.AddSeconds(20) < DateTime.Now)
            {
                client.SendPacket(new KeepAlivePacket(MathHelper.Random.Next()));
                client.LastKeepAliveSent = DateTime.Now;
                // TODO: Confirm keep alive
            }
            if (client.BlockBreakStartTime != null)
            {
                byte progress = (byte)((DateTime.Now - client.BlockBreakStartTime.Value).TotalMilliseconds / client.BlockBreakStageTime);
                var knownClients = EntityManager.GetKnownClients(client.Entity);
                foreach (var c in knownClients)
                {
                    c.SendPacket(new BlockBreakAnimationPacket(client.Entity.EntityId,
                        client.ExpectedBlockToMine.X, client.ExpectedBlockToMine.Y, client.ExpectedBlockToMine.Z, progress));
                }
            }
            if (NextChunkUpdate < DateTime.Now) // Once per second
            {
                // Update chunks
                if (client.Settings.ViewDistance < client.Settings.MaxViewDistance)
                {
                    client.Settings.ViewDistance++;
                    client.ForceUpdateChunksAsync();
                }
            }
        }

        #endregion

        #region Event handlers

        private void WorldBlockChange(object sender, BlockChangeEventArgs e)
        {
            var world = sender as World;
            var chunk = world.FindChunk(e.Coordinates);
            var chunkCoordinates = new Coordinates2D(chunk.X, chunk.Z);
            var block = world.GetBlock(e.Coordinates);
			Block.DoBlockUpdates(world, e.Coordinates);
            foreach (var client in GetClientsInWorld(world))
            {
                if (client.LoadedChunks.Contains(chunkCoordinates))
                {
                    client.SendPacket(new BlockChangePacket(e.Coordinates.X, (byte)e.Coordinates.Y, e.Coordinates.Z,
                        block.Id, block.Metadata));
                }
            }
        }

        // TODO: Find a better way
        private void WorldSpawnEntityRequested(object sender, SpawnEntityEventArgs e)
        {
            if (e.Entity is Entity)
                EntityManager.SpawnEntity(sender as World, e.Entity as Entity);
        }

        #endregion

        #region Events

        public event EventHandler<ChatMessageEventArgs> ChatMessage;
        protected internal virtual void OnChatMessage(ChatMessageEventArgs e)
        {
            if (ChatMessage != null) ChatMessage(this, e);
        }

		public event EventHandler<ConnectionEstablishedEventArgs> ConnectionEstablished;
		protected internal virtual void OnConnectionEstablished(ConnectionEstablishedEventArgs e)
		{
			if (ConnectionEstablished != null) ConnectionEstablished(this, e);
		}

        public event EventHandler<PlayerLogInEventArgs> PlayerLoggedIn;
        protected internal virtual void OnPlayerLoggedIn(PlayerLogInEventArgs e)
        {
            if (PlayerLoggedIn != null) PlayerLoggedIn(this, e);
        }

        public event EventHandler<PlayerLogInEventArgs> PlayerLoggedOut;
        protected internal virtual void OnPlayerLoggedOut(PlayerLogInEventArgs e)
        {
            if (PlayerLoggedOut != null) PlayerLoggedOut(this, e);
        }

        #endregion
    }
}
