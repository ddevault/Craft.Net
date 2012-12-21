using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading;
using Craft.Net.Server.Events;
using Craft.Net.Server.Packets;
using Craft.Net.Data;
using Craft.Net.Data.Entities;

namespace Craft.Net.Server
{
    public class MinecraftServer
    {
        /// <summary>
        /// The protocol version supported by this server.
        /// </summary>
        public const int ProtocolVersion = 51;
        public const string TargetClientVersion = "1.4.6";

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
        /// This server's entity manager.
        /// </summary>
        public EntityManager EntityManager { get; set; }
        /// <summary>
        /// This server's weather manager.
        /// </summary>
        public WeatherManager WeatherManager { get; set; }
        /// <summary>
        /// The socket this server listens on.
        /// </summary>
        public Socket Socket { get; set; }
        /// <summary>
        /// Settings that describe this server's function.
        /// </summary>
        public ServerSettings Settings { get; set; }

        protected internal Dictionary<string, PluginChannel> PluginChannels { get; set; }

        internal RSACryptoServiceProvider CryptoServiceProvider { get; set; }
        internal RSAParameters ServerKey { get; set; }

        private AutoResetEvent sendQueueReset { get; set; } // TODO: Move packet sending to individual clients
        private Thread sendQueueThread { get; set; }
        private Timer updatePlayerListTimer { get; set; }

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
            WeatherManager = new WeatherManager(this);
            // Bind socket
            Socket = new Socket(AddressFamily.InterNetwork,
                                SocketType.Stream, ProtocolType.Tcp);
            Socket.Bind(endPoint);
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

            Socket.Listen(10);
            sendQueueReset = new AutoResetEvent(false);
            sendQueueThread = new Thread(SendQueueWorker);
            sendQueueThread.Start();
            Socket.BeginAccept(AcceptConnectionAsync, null);

            updatePlayerListTimer = new Timer(UpdatePlayerList, null, 60000, 60000);

            LogProvider.Log("Server started.");
        }

        /// <summary>
        /// Stops the server.
        /// </summary>
        public void Stop()
        {
            LogProvider.Log("Stopping server...");
            if (sendQueueThread != null)
            {
                sendQueueThread.Abort();
                sendQueueThread = null;
            }
            if (Socket != null)
            {
                if (Socket.Connected)
                    Socket.Shutdown(SocketShutdown.Both);
                Socket = null;
            }
            updatePlayerListTimer.Dispose();
            LogProvider.Log("Server stopped.");
        }

        /// <summary>
        /// After queueing several packets to send, this will
        /// process the queue.
        /// </summary>
        public void ProcessSendQueue()
        {
            if (sendQueueReset != null)
                sendQueueReset.Set();
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
            Levels.Add(level);
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
            ProcessSendQueue();
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
            client.SendPacket(new LoginPacket(client.Entity.Id,
                                              DefaultWorld.LevelType, DefaultLevel.GameMode,
                                              client.Entity.Dimension, Settings.Difficulty,
                                              Settings.MaxPlayers));

            // Send initial chunks
            client.UpdateChunks(true);
            client.SendPacket(new PlayerPositionAndLookPacket(
                                  client.Entity.Position, client.Entity.Yaw, client.Entity.Pitch, true));
            client.SendQueue.Last().OnPacketSent += (sender, e) => { client.ReadyToSpawn = true; };

            // Send entities
            EntityManager.SendClientEntities(client);

            client.SendPacket(new SetWindowItemsPacket(0, client.Entity.Inventory.GetSlots()));
            client.SendPacket(new UpdateHealthPacket(client.Entity.Health, client.Entity.Food, client.Entity.FoodSaturation));
            client.SendPacket(new SpawnPositionPacket(client.Entity.SpawnPoint));
            client.SendPacket(new TimeUpdatePacket(DefaultLevel.Time));

            UpdatePlayerList(null); // Should also process send queue

            var args = new PlayerLogInEventArgs(client);
            OnPlayerLoggedIn(args);
            LogProvider.Log(client.Username + " logged in.");
            if (!args.Handled)
                SendChat(client.Username + " logged in.");

            client.StartWorkers();
        }

        void EntityInventoryChanged(object sender, Data.Events.InventoryChangedEventArgs e)
        {
            // Send changes to client
            var client = EntityManager.GetClient(sender as PlayerEntity);
            client.SendPacket(new SetSlotPacket(0, e.Index, e.NewValue));
            ProcessSendQueue();
        }

        protected internal void UpdatePlayerList(object unused)
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
            ProcessSendQueue();
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

        #endregion

        #endregion

        #region Private Methods

        private void HandleOnBlockChanged(object sender, BlockChangedEventArgs e)
        {
            foreach (MinecraftClient client in EntityManager.GetClientsInWorld(e.World))
                client.SendPacket(new BlockChangePacket(e.Position, e.Value));
            ProcessSendQueue();
        }

        private void SendQueueWorker()
        {
            while (true)
            {
                sendQueueReset.Reset();
                sendQueueReset.WaitOne();
                if (Clients.Count != 0)
                {
                    lock (Clients)
                    {
                        for (int i = 0; i < Clients.Count; i++)
                        {
                            while (i < Clients.Count && Clients[i].SendQueue.Count != 0)
                            {
                                Packet packet;
                                while (!Clients[i].SendQueue.TryDequeue(out packet)) { }
#if DEBUG
                                LogProvider.Log("[SERVER->CLIENT] " + Clients[i].Socket.RemoteEndPoint,
                                    LogImportance.Low);
                                LogProvider.Log(packet.ToString(), LogImportance.Low);
#endif
                                try
                                {
                                    packet.SendPacket(this, Clients[i]);
                                    packet.FirePacketSent();
                                    OnPacketSent(new PacketEventArgs(packet, Clients[i], this));
                                }
                                catch
                                {
                                    if (i < Clients.Count)
                                    {
                                        Clients[i].IsDisconnected = true;
                                        if (Clients[i].Socket.Connected)
                                            Clients[i].Socket.BeginDisconnect(false, null, null);
                                    }
                                    i--;
                                    break;
                                }
                            }
                        }
                    }
                }
                Thread.Sleep(1);
            }
        }

        protected void AcceptConnectionAsync(IAsyncResult result)
        {
            Socket connection = Socket.EndAccept(result);
            var client = new MinecraftClient(connection, this);
            Clients.Add(client);
            client.Socket.SendTimeout = 5000;
            client.Socket.BeginReceive(client.RecieveBuffer, client.RecieveBufferIndex,
                                       client.RecieveBuffer.Length,
                                       SocketFlags.None, SocketRecieveAsync, client);
            Socket.BeginAccept(AcceptConnectionAsync, null);
        }

        protected void SocketRecieveAsync(IAsyncResult result)
        {
            var client = (MinecraftClient)result.AsyncState;
            SocketError error;
            int length = client.Socket.EndReceive(result, out error) + client.RecieveBufferIndex;
            if (error != SocketError.Success || !client.Socket.Connected || length == client.RecieveBufferIndex)
            {
                if (error != SocketError.Success)
                    LogProvider.Log("Socket error: " + error);
                client.IsDisconnected = true;
            }
            else
            {
                try
                {
                    IEnumerable<Packet> packets = PacketReader.TryReadPackets(client, length);
                    foreach (Packet packet in packets)
                    {
                        OnPacketRecieved(new PacketEventArgs(packet, client, this));
                        packet.HandlePacket(this, client);
                    }

                    if (!client.IsDisconnected)
                    {
                        client.Socket.BeginReceive(client.RecieveBuffer, client.RecieveBufferIndex,
                                                   client.RecieveBuffer.Length - client.RecieveBufferIndex,
                                                   SocketFlags.None, SocketRecieveAsync, client);
                    }
                }
                catch (InvalidOperationException e)
                {
                    client.IsDisconnected = true;
                    LogProvider.Log("Disconnected client with protocol error. " + e.Message);
                }
                catch (NotImplementedException)
                {
                    client.IsDisconnected = true;
                    LogProvider.Log("Disconnected client using unsupported features.");
                }
                catch (Exception e)
                {
                    client.IsDisconnected = true;
                    LogProvider.Log("Disconnected client with error: " + e.Message);
                }
            }
            if (client.IsDisconnected)
            {
                lock (Clients)
                {
                    if (client.Socket.Connected)
                        client.Socket.BeginDisconnect(false, null, null);
                    if (client.KeepAliveTimer != null)
                        client.KeepAliveTimer.Dispose();
                    if (client.IsLoggedIn)
                    {
                        foreach (MinecraftClient remainingClient in Clients)
                        {
                            if (remainingClient.IsLoggedIn)
                            {
                                remainingClient.SendPacket(new PlayerListItemPacket(
                                                               client.Username, false, 0));
                            }
                        }
                        DefaultLevel.SavePlayer(client.Entity);
                        var args = new PlayerLogInEventArgs(client);
                        OnPlayerLoggedOut(args);
                        if (!args.Handled)
                            SendChat(client.Username + " logged out.");
                        EntityManager.DespawnEntity(client.Entity);
                    }
                    Clients.Remove(client);
                }
                ProcessSendQueue();
            }
        }

        #endregion
    }
}