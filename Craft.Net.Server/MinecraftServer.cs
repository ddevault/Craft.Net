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
using Craft.Net.Data.Blocks;

namespace Craft.Net.Server
{
    /// <summary>
    /// A Minecraft 12w34b server.
    /// </summary>
    public class MinecraftServer
    {
        #region Public Fields

        /// <summary>
        /// The protocol version supported by this server.
        /// </summary>
        public const int ProtocolVersion = 42;

        /// <summary>
        /// A list of all connected clients. Not all connected
        /// clients will be logged in.
        /// </summary>
        public List<MinecraftClient> Clients;
        /// <summary>
        /// The default world to spawn clients in.
        /// </summary>
        public int DefaultWorldIndex;
        /// <summary>
        /// Set to true if this server is to use encrypted
        /// connections.
        /// </summary>
        public bool EncryptionEnabled;
        /// <summary>
        /// A list of <see cref="ILogProvider"/> objects to log
        /// data to.
        /// </summary>
        public List<ILogProvider> LogProviders;
        /// <summary>
        /// The maximum number of players that may log in.
        /// </summary>
        public byte MaxPlayers;
        /// <summary>
        /// The message of the day.
        /// </summary>
        public string MotD;
        /// <summary>
        /// Set to true to authenticate connecting users with Minecraft.net
        /// </summary>
        public bool OnlineMode;
        /// <summary>
        /// A list of Worlds this server will use.
        /// </summary>
        public List<Level> Levels;
        /// <summary>
        /// This server's entity manager.
        /// </summary>
        public EntityManager EntityManager;
        /// <summary>
        /// This server's difficulty.
        /// </summary>
        public Difficulty Difficulty;

        #endregion

        #region Private Fields

        internal static Random Random;
        internal RSACryptoServiceProvider CryptoServiceProvider;
        internal Dictionary<string, PluginChannel> PluginChannels;
        internal RSAParameters ServerKey;

        private AutoResetEvent sendQueueReset;
        private Thread sendQueueThread;
        private Timer updatePlayerListTimer;
        private Socket socket;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the default world for new clients.
        /// </summary>
        public World DefaultWorld
        {
            get { return Levels[DefaultWorldIndex].World; }
        }

        /// <summary>
        /// Gets the default level for new clients.
        /// </summary>
        public Level DefaultLevel
        {
            get { return Levels[DefaultWorldIndex]; }
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

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new Minecraft server to listen on the requested
        /// endpoint.
        /// </summary>
        public MinecraftServer(IPEndPoint endPoint)
        {
            Clients = new List<MinecraftClient>();
            MaxPlayers = 25;
            MotD = "Craft.Net Server";
            OnlineMode = EncryptionEnabled = true;
            Random = new Random();
            DefaultWorldIndex = 0;
            Levels = new List<Level>();
            LogProviders = new List<ILogProvider>();
            PluginChannels = new Dictionary<string, PluginChannel>();
            EntityManager = new EntityManager(this);
            Difficulty = Difficulty.Peaceful;

            socket = new Socket(AddressFamily.InterNetwork,
                                SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(endPoint);
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
                Log("Unable to start server with no worlds loaded.");
                throw new InvalidOperationException("Unable to start server with no worlds loaded.");
            }

            Log("Starting Craft.Net server...");

            CryptoServiceProvider = new RSACryptoServiceProvider(1024);
            ServerKey = CryptoServiceProvider.ExportParameters(true);

            socket.Listen(10);
            sendQueueReset = new AutoResetEvent(false);
            sendQueueThread = new Thread(SendQueueWorker);
            sendQueueThread.Start();
            socket.BeginAccept(AcceptConnectionAsync, null);

            updatePlayerListTimer = new Timer(UpdatePlayerList, null, 60000, 60000);

            Log("Server started.");
        }

        /// <summary>
        /// Stops the server.
        /// </summary>
        public void Stop()
        {
            Log("Stopping server...");
            if (sendQueueThread != null)
            {
                sendQueueThread.Abort();
                sendQueueThread = null;
            }
            if (socket != null)
            {
                if (socket.Connected)
                    socket.Shutdown(SocketShutdown.Both);
                socket = null;
            }
            updatePlayerListTimer.Dispose();
            Log("Server stopped.");
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
        /// Adds the requested <see cref="ILogProvider"/> to the
        /// list of log providers.
        /// </summary>
        public void AddLogProvider(ILogProvider logProvider)
        {
            LogProviders.Add(logProvider);
        }

        /// <summary>
        /// Logs the given text with high importance.
        /// </summary>
        public void Log(string text)
        {
            Log(text, LogImportance.High);
        }

        /// <summary>
        /// Logs the given text.
        /// </summary>
        public void Log(string text, LogImportance logLevel)
        {
            foreach (ILogProvider provider in LogProviders)
                provider.Log(text, logLevel);
        }

        /// <summary>
        /// Adds a world to this server's list of worlds.
        /// </summary>
        public void AddLevel(Level level)
        {
            level.World.OnBlockChanged += HandleOnBlockChanged;
            Levels.Add(level);
        }

        /// <summary>
        /// Gets the World that the given client is present in.
        /// </summary>
        public World GetClientWorld(MinecraftClient client)
        {
            return DefaultWorld; // TODO
        }

        /// <summary>
        /// Gets all <see cref="MinecraftClient"/> objects in the given
        /// world.
        /// </summary>
        public IEnumerable<MinecraftClient> GetClientsInWorld(World world)
        {
            return EntityManager.GetClientsInWorld(world);
        }

        /// <summary>
        /// Sends the specified chat message to all connected clients.
        /// </summary>
        public void SendChat(string message)
        {
            for (int i = 0; i < Clients.Count; i++)
                Clients[i].SendPacket(new ChatMessagePacket(message));
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

        /// <summary>
        /// Sends and updated player list to all connected clients.
        /// </summary>
        public void UpdatePlayerList(object unused)
        {
            if (Clients.Count != 0)
            {
                for (int i = 0; i < Clients.Count; i++)
                {
                    foreach (MinecraftClient client in Clients)
                        Clients[i].SendPacket(new PlayerListItemPacket(
                                                  client.Username, true, client.Ping));
                }
            }
            ProcessSendQueue();
        }

        /// <summary>
        /// Spawns a bolt of lightning in the given world
        /// at the given position.
        /// </summary>
        public void SpawnLightning(World world, Vector3 position)
        {
            var chunk = world.GetChunk(World.WorldToChunkCoordinates(position));
            var block = World.FindBlockPosition(position);
            byte y = (byte)(chunk.GetHeight((byte)block.X, (byte)block.Z) + 1);

            var strike = new Vector3(position.X, y, position.Z);
            if (world.GetBlock(strike + Vector3.Down).Transparency == Transparency.Opaque)
                world.SetBlock(strike, new FireBlock());

            var clients = GetClientsInWorld(world);
            foreach (var minecraftClient in clients)
                minecraftClient.SendPacket(new SpawnLightningPacket(EntityManager.nextEntityId++, strike));

            ProcessSendQueue();
        }

        #endregion

        #region Private Methods

        private void HandleOnBlockChanged(object sender, BlockChangedEventArgs e)
        {
            foreach (MinecraftClient client in GetClientsInWorld(e.World))
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
                                Packet packet = Clients[i].SendQueue.Dequeue();
                                // I don't understand why this happens. The only place the SendQueue
                                // is modified is through Client.SendPacket, and I watched for null packets
                                // entering through there. There is never a null packet that is added
                                // into the packet queue
                                if (packet == null)
                                    continue;
#if DEBUG
                                Log("[SERVER->CLIENT] " + Clients[i].Socket.RemoteEndPoint,
                                    LogImportance.Low);
                                Log(packet.ToString(), LogImportance.Low);
#endif
                                try
                                {
                                    packet.SendPacket(this, Clients[i]);
                                    packet.FirePacketSent();
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

        private void AcceptConnectionAsync(IAsyncResult result)
        {
            Socket connection = socket.EndAccept(result);
            var client = new MinecraftClient(connection, this);
            Clients.Add(client);
            client.Socket.SendTimeout = 5000;
            client.Socket.BeginReceive(client.RecieveBuffer, client.RecieveBufferIndex,
                                       client.RecieveBuffer.Length,
                                       SocketFlags.None, SocketRecieveAsync, client);
            socket.BeginAccept(AcceptConnectionAsync, null);
        }

        private void SocketRecieveAsync(IAsyncResult result)
        {
            var client = (MinecraftClient)result.AsyncState;
            SocketError error;
            int length = client.Socket.EndReceive(result, out error) + client.RecieveBufferIndex;
            if (error != SocketError.Success || !client.Socket.Connected || length == client.RecieveBufferIndex)
            {
                if (error != SocketError.Success)
                    Log("Socket error: " + error);
                client.IsDisconnected = true;
            }
            else
            {
                try
                {
                    IEnumerable<Packet> packets = PacketReader.TryReadPackets(ref client, length);
                    foreach (Packet packet in packets)
                        packet.HandlePacket(this, client);

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
                    Log("Disconnected client with protocol error. " + e.Message);
                }
                catch (NotImplementedException)
                {
                    client.IsDisconnected = true;
                    Log("Disconnected client using unsupported features.");
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

        #region Internal Methods

        internal void FireOnChatMessage(ChatMessageEventArgs e)
        {
            if (ChatMessage != null)
                ChatMessage(this, e);
        }

        internal void LogInPlayer(MinecraftClient client)
        {
            client.IsLoggedIn = true;
            // Spawn player
            client.Entity = new PlayerEntity();
            client.Entity.Position = DefaultLevel.SpawnPoint;
            client.Entity.Position += new Vector3(0, PlayerEntity.Height, 0);
            client.Entity.GameMode = DefaultLevel.GameMode;
            client.Entity.InventoryChanged += Entity_InventoryChanged;
            EntityManager.SpawnEntity(DefaultWorld, client.Entity);
            client.SendPacket(new LoginPacket(client.Entity.Id,
                                              DefaultWorld.LevelType, DefaultLevel.GameMode,
                                              client.Entity.Dimension, this.Difficulty,
                                              MaxPlayers));

            // Send initial chunks
            client.UpdateChunks(true);
            client.SendPacket(new PlayerPositionAndLookPacket(
                                  client.Entity.Position, client.Entity.Yaw, client.Entity.Pitch, true));
            client.SendQueue.Last().OnPacketSent += (sender, e) => { client.ReadyToSpawn = true; };

            // Send entities
            EntityManager.SendClientEntities(client);

            UpdatePlayerList(null); // Should also process send queue

            var args = new PlayerLogInEventArgs(client);
            OnPlayerLoggedIn(args);
            Log(client.Username + " logged in.");
            if (!args.Handled)
                SendChat(client.Username + " logged in.");
        }

        void Entity_InventoryChanged(object sender, Data.Events.InventoryChangedEventArgs e)
        {
            var client = EntityManager.GetClient(sender as PlayerEntity);
            client.SendPacket(new SetSlotPacket(0, e.Index, e.NewValue));
            this.ProcessSendQueue();
        }

        #region Events

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

        #endregion

        #endregion
    }
}