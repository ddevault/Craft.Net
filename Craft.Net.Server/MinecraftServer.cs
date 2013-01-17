using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading;
using Craft.Net.Server.Events;
using Craft.Net.Data;
using Craft.Net.Data.Entities;
using Craft.Net.Server.Handlers;

namespace Craft.Net.Server
{
    public class MinecraftServer
    {
        static MinecraftServer()
        {
            PacketHandlerDelegates = new PacketHandler[256];
            PacketHandlers.RegisterHandlers();
        }

        public const string TargetClientVersion = "13w03a";

        public delegate void PacketHandler(MinecraftClient client, MinecraftServer server, IPacket packet);

        #region Properties

        /// <summary>
        /// A list of all connected clients. Not all connected
        /// clients will be logged in.
        /// </summary>
        public List<MinecraftClient> Clients { get; set; }
        /// <summary>
        /// A list of <see cref="ILogProvider"/> objects to log
        /// data to.
        /// </summary>
        public List<ILogProvider> LogProviders { get; set; }
        /// <summary>
        /// A list of Worlds this server will use.
        /// </summary>
        public List<Level> Levels { get; set; }
        /// <summary>
        /// Weather managers associated with each level.
        /// </summary>
        public List<WeatherManager> WeatherManagers { get; set; }
        /// <summary>
        /// This server's entity manager.
        /// </summary>
        public EntityManager EntityManager { get; set; }
        /// <summary>
        /// Settings that describe this server's function.
        /// </summary>
        public ServerSettings Settings { get; set; }
        /// <summary>
        /// The TCP listner this server users for incoming connections.
        /// </summary>
        public TcpListener Listener { get; set; }

        protected internal Dictionary<string, PluginChannel> PluginChannels { get; set; }

        internal RSACryptoServiceProvider CryptoServiceProvider { get; set; }
        internal RSAParameters ServerKey { get; set; }

        private Timer updatePlayerListTimer { get; set; }
        private Thread NetworkWorkerThread { get; set; }
        private object NetworkLock;
        private static PacketHandler[] PacketHandlerDelegates { get; set; }

        /// <summary>
        /// Gets the default world for new clients.
        /// </summary>
        public World DefaultWorld
        {
            get { return Levels[Settings.DefaultWorldIndex].World; }
        }

        /// <summary>
        /// Gets the default level for new clients.
        /// </summary>
        public Level DefaultLevel
        {
            get { return Levels[Settings.DefaultWorldIndex]; }
        }

        #endregion

        #region Events

        /// <summary>
        /// Fired when a player logs in.
        /// </summary>
        public event EventHandler<PlayerLogInEventArgs> PlayerLoggedIn;
        /// <summary>
        /// Fired when a player logs out.
        /// </summary>
        public event EventHandler<PlayerLogInEventArgs> PlayerLoggedOut;
        /// <summary>
        /// Fired when the server recieves a <see cref="ChatMessagePacket"/>.
        /// </summary>
        public event EventHandler<ChatMessageEventArgs> ChatMessage;
        /// <summary>
        /// Fired after a packet has been send from server to client.
        /// </summary>
        public event EventHandler<PacketEventArgs> PacketSent;
        /// <summary>
        /// Fired after the server recieves a packet from a client.
        /// </summary>
        public event EventHandler<PacketEventArgs> PacketRecieved;
        /// <summary>
        /// Fired when a player dies.
        /// </summary>
        public event EventHandler<PlayerDeathEventArgs> PlayerDeath;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Minecraft server to listen on the requested
        /// endpoint.
        /// </summary>
        public MinecraftServer(IPEndPoint endPoint) : this(endPoint, ServerSettings.DefaultSettings)
        {
        }

        /// <summary>
        /// Creates a new Minecraft server to listen on the requested
        /// endpoint.
        /// </summary>
        public MinecraftServer(IPEndPoint endPoint, ServerSettings settings)
        {
            Settings = settings;
            // Initialize variables
            Clients = new List<MinecraftClient>();
            Levels = new List<Level>();
            LogProviders = new List<ILogProvider>();
            PluginChannels = new Dictionary<string, PluginChannel>();
            EntityManager = new EntityManager(this);
            WeatherManagers = new List<WeatherManager>();
            // Bind socket
            Listener = new TcpListener(endPoint);
            NetworkLock = new object();
            World.RecreateLightIndex();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts the server.
        /// </summary>
        public void Start()
        {
            if (Levels.Count == 0)
            {
                LogProvider.Log("Unable to start server with no worlds loaded.");
                throw new InvalidOperationException("Unable to start server with no worlds loaded.");
            }

            LogProvider.Log("Starting Craft.Net server...");

            CryptoServiceProvider = new RSACryptoServiceProvider(1024);
            ServerKey = CryptoServiceProvider.ExportParameters(true);

            Listener.Start();
            Listener.BeginAcceptTcpClient(AcceptConnectionAsync, null);

            NetworkWorkerThread = new Thread(NetworkWorker);
            NetworkWorkerThread.Start();

            updatePlayerListTimer = new Timer(UpdatePlayerList, null, 60000, 60000);

            LogProvider.Log("Server started.");
        }

        /// <summary>
        /// Stops the server.
        /// </summary>
        public void Stop()
        {
            LogProvider.Log("Stopping server...");
            if (Listener != null)
            {
                Listener.Stop();
                Listener = null;
            }
            if (NetworkWorkerThread != null)
            {
                NetworkWorkerThread.Abort();
                NetworkWorkerThread = null;
            }
            updatePlayerListTimer.Dispose();
            LogProvider.Log("Server stopped.");
        }

        /// <summary>
        /// Adds a worldBlockChangedr's list of worlds.
        /// </summary>
        public void AddLevel(Level level)
        {
            level.World.BlockChanged += HandleOnBlockChanged;
            level.World.SpawnEntity += (sender, args) => 
                EntityManager.SpawnEntity(sender as World, args.Entity);
            level.World.DestroyEntity += (sender, args) =>
                EntityManager.DespawnEntity(sender as World, args.Entity);
            WeatherManagers.Add(new WeatherManager(level.World, this));
            Levels.Add(level);
        }

        public WeatherManager GetWeatherManagerForWorld(World world)
        {
            return WeatherManagers.FirstOrDefault(w => w.World == world);
        }

        /// <summary>
        /// Gets the level that handles the specified world.
        /// </summary>
        public Level GetLevel(World world)
        {
            return Levels.First(l => l.World == world);
        }

        /// <summary>
        /// Gets the level that the specified client resides in.
        /// </summary>
        public Level GetLevel(MinecraftClient client)
        {
            return GetLevel(client.World);
        }

        /// <summary>
        /// Sends the specified chat message to all connected clients.
        /// </summary>
        public void SendChat(string message)
        {
            for (int i = 0; i < Clients.Count; i++)
            {
                if (Clients[i].IsLoggedIn)
                    Clients[i].SendPacket(new ChatMessagePacket(message));
            }
        }

        /// <summary>
        /// Registers the provided <see cref="PluginChannel"/> to listen
        /// for and send plugin messages.
        /// </summary>
        public void RegisterPluginChannel(PluginChannel channel)
        {
            PluginChannels.Add(channel.Channel, channel);
            channel.ChannelRegistered(this);
        }

        public MinecraftClient GetClient(string name)
        {
            return Clients.FirstOrDefault(c => c.Username == name && c.IsLoggedIn);
        }

        public static void RegisterPacketHandler(byte packetId, PacketHandler handler)
        {
            PacketHandlerDelegates[packetId] = handler;
        }

        #endregion

        #region Internal Methods

        internal void LogInPlayer(MinecraftClient client)
        {
            client.IsLoggedIn = true;
            // Spawn player
            client.Entity = DefaultLevel.LoadPlayer(client.Username);
            client.Entity.Username = client.Username;
            client.Entity.InventoryChanged += EntityInventoryChanged;
            EntityManager.SpawnEntity(DefaultWorld, client.Entity);
            client.SendPacket(new LoginRequestPacket(client.Entity.Id,
                                              DefaultWorld.LevelType, client.Entity.GameMode,
                                              client.Entity.Dimension, Settings.Difficulty,
                                              Settings.MaxPlayers));
            client.SendPacket(new SpawnPositionPacket((int)client.Entity.SpawnPoint.X, (int)client.Entity.SpawnPoint.Y, (int)client.Entity.SpawnPoint.Z));
            client.SendPacket(new TimeUpdatePacket(DefaultLevel.Time, DefaultLevel.Time));
            UpdatePlayerList(null);
            client.SendPacket(new SetWindowItemsPacket(0, client.Entity.Inventory.GetSlots()));

            // Send initial chunks
            client.UpdateChunks(true);
            client.SendPacket(new PlayerPositionAndLookPacket(client.Entity.Position.X, client.Entity.Position.Y + client.Entity.Size.Height,
                client.Entity.Position.Z, client.Entity.Position.Y - client.Entity.Size.Height, client.Entity.Yaw, client.Entity.Pitch, true));
            // TODO: Move 1.62 somewhere else

            // Send entities
            EntityManager.SendClientEntities(client);

            client.SendPacket(new UpdateHealthPacket(client.Entity.Health, client.Entity.Food, client.Entity.FoodSaturation));

            var args = new PlayerLogInEventArgs(client);
            OnPlayerLoggedIn(args);
            LogProvider.Log(client.Username + " joined the game.");
            if (!args.Handled)
                SendChat(ChatColors.Yellow + client.Username + " joined the game.");
        }

        protected internal void UpdatePlayerList(object discarded)
        {
            if (Clients.Count != 0)
            {
                for (int i = 0; i < Clients.Count; i++)
                {
                    foreach (MinecraftClient client in Clients)
                    {
                        if (client.IsLoggedIn)
                            Clients[i].SendPacket(new PlayerListItemPacket(client.Username, true, client.Ping));
                    }
                }
            }
        }

        #region Events

        protected internal virtual void OnChatMessage(ChatMessageEventArgs e)
        {
            if (ChatMessage != null)
                ChatMessage(this, e);
        }

        protected internal virtual void OnPlayerLoggedIn(PlayerLogInEventArgs e)
        {
            if (PlayerLoggedIn != null)
                PlayerLoggedIn(this, e);
        }

        protected internal virtual void OnPlayerLoggedOut(PlayerLogInEventArgs e)
        {
            if (PlayerLoggedOut != null)
                PlayerLoggedOut(this, e);
            e.Client.World.Level.SavePlayer(e.Client.Entity);
            if (!e.Handled)
            {
                LogProvider.Log(e.Client.Username + " left the game.", LogImportance.High);
                SendChat(ChatColors.Yellow + e.Client.Username + " left the game.");
            }
        }

        protected internal virtual void OnPacketSent(PacketEventArgs e)
        {
            if (PacketSent != null)
                PacketSent(this, e);
        }

        protected internal virtual void OnPacketRecieved(PacketEventArgs e)
        {
            if (PacketRecieved != null)
                PacketRecieved(this, e);
        }

        protected internal virtual void OnPlayerDeath(PlayerDeathEventArgs e)
        {
            if (PlayerDeath != null)
                PlayerDeath(this, e);
            if (!e.Handled)
            {
                switch (e.DeathType)
                {
                    case DamageType.Combat:
                        if (e.Killer is PlayerEntity)
                        {
                            var killer = e.Killer as PlayerEntity;
                            SendChat(e.Player.Username + " was killed by " + killer.Username);
                        }
                        else
                            SendChat(e.Player.Username + " died.");
                        // TODO: Mobs
                        break;
                    default:
                        SendChat(e.Player.Username + " died.");
                        break;
                }
            }
        }

        #region Handlers

        void EntityInventoryChanged(object sender, Data.Events.InventoryChangedEventArgs e)
        {
            // Send changes to client
            var client = EntityManager.GetClient(sender as PlayerEntity);
            client.SendPacket(new SetSlotPacket(0, e.Index, e.NewValue));
        }

        #endregion

        #endregion

        #endregion

        #region Private Methods

        private void HandleOnBlockChanged(object sender, BlockChangedEventArgs e)
        {
            foreach (MinecraftClient client in EntityManager.GetClientsInWorld(e.World))
                client.SendPacket(new BlockChangePacket((int)e.Position.X, (byte)e.Position.Y, (int)e.Position.Z,
                    e.Value.Id, e.Value.Metadata));
        }

        private void AcceptConnectionAsync(IAsyncResult result)
        {
            try
            {
                if (Listener == null || result == null) return; // This happens sometimes on shut down, it's weird
                var tcpClient = Listener.EndAcceptTcpClient(result);
                var client = new MinecraftClient(tcpClient, this);
                lock (NetworkLock)
                {
                    Clients.Add(client);
                }
                Listener.BeginAcceptTcpClient(AcceptConnectionAsync, null);
            }
            catch { } // TODO: Investigate this more deeply
        }

        private void NetworkWorker()
        {
            while (true) // TODO: Consider refactoring
            {
                lock (NetworkLock)
                {
                    for (int i = 0; i < Clients.Count; i++)
                    {
                        var client = Clients[i];
                        if (client.IsLoggedIn)
                            DoClientUpdates(client);
                        bool disconnect = false;
                        while (client.SendQueue.Count != 0)
                        {
                            IPacket packet;
                            if (client.SendQueue.TryDequeue(out packet))
                            {
                                try
                                {
                                    packet.WritePacket(client.Stream);
#if DEBUG
                                    LogProvider.Log(packet, false);
#endif
                                    client.Stream.Flush();
                                }
                                catch
                                {
                                    // TODO: Consider more detail
                                    disconnect = true;
                                    break;
                                }
                                if (packet is DisconnectPacket)
                                {
                                    disconnect = true;
                                    break;
                                }
                                if (packet is EncryptionKeyResponsePacket)
                                {
                                    // Set up crypto stream
#if DEBUG
                                    LogProvider.Log("Encryption enabled with " + client.Username, LogImportance.Low);
#endif
                                    client.Stream = new MinecraftStream(new BufferedStream(new AesStream(client.NetworkStream, client.SharedKey)));
                                    client.EncryptionEnabled = true;
                                }
                            }
                        }
                        if (disconnect)
                        {
                            Clients.Remove(client);
                            i--;
                            EntityManager.DespawnEntity(client.Entity);
                            if (client.IsLoggedIn)
                                OnPlayerLoggedOut(new PlayerLogInEventArgs(client));
                            continue;
                        }
                        // Each client has a maximum of 10 milliseconds per iteration for reads
                        DateTime readTimeout = DateTime.Now.AddMilliseconds(10);
                        while (client.NetworkStream.DataAvailable && DateTime.Now < readTimeout)
                        {
                            try
                            {
                                var packet = PacketReader.ReadPacket(client.Stream);
#if DEBUG
                                LogProvider.Log(packet, true);
#endif
                                if (packet is DisconnectPacket) // TODO: Should we have a disconnect packet handler?
                                {
                                    Clients.Remove(client);
                                    i--;
                                    EntityManager.DespawnEntity(client.Entity);
                                    if (client.IsLoggedIn)
                                        OnPlayerLoggedOut(new PlayerLogInEventArgs(client));
                                    continue;
                                }
                                HandlePacket(client, packet);
                                if (client.DisconnectPending)
                                    break;
                            }
                            catch (InvalidOperationException e)
                            {
                                new DisconnectPacket(e.Message).WritePacket(client.Stream);
                                client.Stream.Flush();
                                Clients.Remove(client);
                                i--;
                                EntityManager.DespawnEntity(client.Entity);
                                if (client.IsLoggedIn)
                                    OnPlayerLoggedOut(new PlayerLogInEventArgs(client));
                            }
                            catch (SocketException e)
                            {
                                Clients.Remove(client);
                                i--;
                                EntityManager.DespawnEntity(client.Entity);
                                if (client.IsLoggedIn)
                                    OnPlayerLoggedOut(new PlayerLogInEventArgs(client));
                            }
                            catch (Exception e)
                            {
                                new DisconnectPacket("A network exception occured: " + e.GetType().Name).WritePacket(client.Stream);
                                client.Stream.Flush();
                                Clients.Remove(client);
                                i--;
                                EntityManager.DespawnEntity(client.Entity);
                                if (client.IsLoggedIn)
                                    OnPlayerLoggedOut(new PlayerLogInEventArgs(client));
                            }
                        }
                    }
                }
                if (DefaultLevel.Time % 5 == 0)
                {
                    foreach (var level in Levels)
                        level.World.DoScheduledUpdates();
                }
                Thread.Sleep(1);
            }
        }

        private void DoClientUpdates(MinecraftClient client)
        {
            // Update keep alive, chunks, etc
            if (client.LastKeepAliveSent.AddSeconds(20) < DateTime.Now)
            {
                client.SendPacket(new KeepAlivePacket(MathHelper.Random.Next()));
                client.LastKeepAliveSent = DateTime.Now;
                // TODO: Confirm keep alive
            }
            if (client.World.Level.Time % 20 == 0) // Once per second
            {
                // Update chunks
                if (client.ViewDistance < client.MaxViewDistance)
                {
                    client.ViewDistance++;
                    client.ForceUpdateChunksAsync();
                }
            }
        }

        private void HandlePacket(MinecraftClient client, IPacket packet)
        {
            if (PacketHandlerDelegates[packet.Id] == null)
                throw new InvalidOperationException("No packet handler set for 0x" + packet.Id.ToString("X2"));
            PacketHandlerDelegates[packet.Id](client, this, packet);
        }

        #endregion
    }
}