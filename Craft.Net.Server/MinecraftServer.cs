using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Linq;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using Craft.Net.Server.Worlds;
using java.security;
using Craft.Net.Server.Blocks;
using System.IO;
using Craft.Net.Server.Packets;
using Craft.Net.Server.Events;
using Craft.Net.Server.Worlds.Entities;

namespace Craft.Net.Server
{
	/// <summary>
	/// A Minecraft server.
	/// </summary>
	public class MinecraftServer
	{
        #region Public Fields

        public const int ProtocolVersion = 37;

        public List<MinecraftClient> Clients;
        public List<World> Worlds;
        public int DefaultWorldIndex;
        public string MotD;
        public byte MaxPlayers;
        public bool OnlineMode;
        public List<ILogProvider> LogProviders;

        public event EventHandler<ChatMessageEventArgs> OnChatMessage;
        
        #endregion
        
        #region Private Fields

		private Socket socket;
        private Thread SendQueueThread;
        private Timer UpdatePlayerListTimer;
        private AutoResetEvent SendQueueReset;

        internal static Random Random;
        internal KeyPair KeyPair;

        #endregion

        #region Public Properties

        public World DefaultWorld
        {
            get
            {
                return Worlds[DefaultWorldIndex];
            }
        }

        #endregion
        
        #region Constructor
		
		public MinecraftServer(IPEndPoint EndPoint)
		{
            Clients = new List<MinecraftClient>();
            MaxPlayers = 25;
            MotD = "Craft.Net Server";
            OnlineMode = false;
            Random = new Random();
            DefaultWorldIndex = 0;
            Worlds = new List<World>();
            LogProviders = new List<ILogProvider>();
 
			socket = new Socket(AddressFamily.InterNetwork,
                                SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(EndPoint);
		}
        
        #endregion
		
        #region Public Methods
        
		public void Start()
        {
            if (Worlds.Count == 0)
            {
                Log("Unable to start server with no worlds loaded.");
                throw new InvalidOperationException("Unable to start server with no worlds loaded.");
            }

            Log("Starting Craft.Net server...");
            KeyPairGenerator keyGen = KeyPairGenerator.getInstance("RSA");
            keyGen.initialize(1024);
            KeyPair = keyGen.generateKeyPair();

            socket.Listen(10);
            SendQueueReset = new AutoResetEvent(false);
            SendQueueThread = new Thread(SendQueueWorker);
            SendQueueThread.Start();
            socket.BeginAccept(AcceptConnectionAsync, null);

            UpdatePlayerListTimer = new Timer(UpdatePlayerList, null, 60000, 60000);

            Log("Server started.");
		}

        public void Stop()
        {
            Log("Stopping server...");
            if (SendQueueThread != null)
            {
                SendQueueThread.Abort();
                SendQueueThread = null;
            }
            if (socket != null)
            {
                if (socket.Connected)
                    socket.Shutdown(SocketShutdown.Both);
                socket = null;
            }
            UpdatePlayerListTimer.Dispose();
            Log("Server stopped.");
        }

        public void ProcessSendQueue()
        {
            SendQueueReset.Set();
        }

        public void AddLogProvider(ILogProvider LogProvider)
        {
            LogProviders.Add(LogProvider);
        }

        public void Log(string Text)
        {
            Log(Text, LogImportance.High);
        }

        public void Log(string Text, LogImportance LogLevel)
        {
            foreach (var provider in LogProviders)
                provider.Log(Text, LogLevel);
        }

        public void AddWorld(World World)
        {
            World.EntityManager.Server = this;
            World.OnBlockChanged += HandleOnBlockChanged;
            Worlds.Add(World);
        }

        private void HandleOnBlockChanged(object sender, BlockChangedEventArgs e)
        {
            foreach (var client in GetClientsInWorld(e.World))
                client.SendPacket(new BlockChangePacket(e.Position, e.Value));
            this.ProcessSendQueue();
        }

        public World GetClientWorld(MinecraftClient Client)
        {
            foreach (World world in this.Worlds)
            {
                if (world.EntityManager.Entities.Contains(Client.Entity))
                    return world;
            }
            return null;
        }

        public MinecraftClient[] GetClientsInWorld(World world)
        {
            List<MinecraftClient> clients = new List<MinecraftClient>();
            foreach (var client in Clients)
            {
                if (world.EntityManager.Entities.Contains(client.Entity))
                    clients.Add(client);
            }
            return clients.ToArray();
        }

        public void SendChat(string message)
        {
            for (int i = 0; i < Clients.Count; i++)
                Clients[i].SendPacket(new ChatMessagePacket(message));
            this.ProcessSendQueue();
        }
        
        #endregion
        
        #region Private Methods

        private void SendQueueWorker()
        {
            while (true)
            {
                SendQueueReset.Reset();
                SendQueueReset.WaitOne();
                if (Clients.Count != 0)
                {
                    for (int i = 0; i < Clients.Count; i++)
                    {
                        while (Clients[i].SendQueue.Count != 0)
                        {
                            int oldCount = Clients.Count;
                            var packet = Clients[i].SendQueue.Dequeue();
                            Log("[SERVER->CLIENT] " + Clients[i].Socket.RemoteEndPoint.ToString(),
                                LogImportance.Low);
                            Log(packet.ToString(), LogImportance.Low);
                            if (Clients.Count >= oldCount)
                                packet.SendPacket(this, Clients[i]);
                            if (Clients.Count < oldCount) // In case this client is disconnected
                                break;
                        }
                    }
                }
                Thread.Sleep(1);
            }
        }
        
        private void AcceptConnectionAsync(IAsyncResult result)
        {
            Socket connection = socket.EndAccept(result);
            MinecraftClient client = new MinecraftClient(connection, this);
            Clients.Add(client);
            client.Socket.SendTimeout = 5000;
            client.Socket.BeginReceive(client.RecieveBuffer, client.RecieveBufferIndex,
                                       client.RecieveBuffer.Length,
                                       SocketFlags.None, SocketRecieveAsync, client);
            socket.BeginAccept(AcceptConnectionAsync, null);
        }

        private void SocketRecieveAsync(IAsyncResult result)
        {
            MinecraftClient client = (MinecraftClient)result.AsyncState;
            SocketError error;
            int length = client.Socket.EndReceive(result, out error);
            if (error != SocketError.Success || !client.Socket.Connected || length == 0)
                client.IsDisconnected = true;
            else
            {
                try
                {
                    var packets = PacketReader.TryReadPackets(ref client, length);
                    foreach (var packet in packets)
                    {
                        packet.HandlePacket(this, ref client);
                    }

                    client.Socket.BeginReceive(client.RecieveBuffer, client.RecieveBufferIndex,
                                               client.RecieveBuffer.Length - client.RecieveBufferIndex,
                                               SocketFlags.None, SocketRecieveAsync, client);
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
                if (client.Socket.Connected)
                    client.Socket.BeginDisconnect(false, null, null);
                client.KeepAliveTimer = null;
                Clients.Remove(client);
                foreach (var remainingClient in Clients)
                {
                    remainingClient.SendPacket(new PlayerListItemPacket(
                        client.Username, false, 0));
                }
                this.ProcessSendQueue();
            }
        }

        public void UpdatePlayerList(object unused)
        {
            if (Clients.Count != 0)
            {
                for (int i = 0; i < Clients.Count; i++)
                {
                    foreach (var client in Clients)
                        Clients[i].SendPacket(new PlayerListItemPacket(
                            client.Username, true, client.Ping));
                }
            }
            this.ProcessSendQueue();
        }
        
        #endregion

        #region Internal Methods

        internal void FireOnChatMessage(ChatMessageEventArgs e)
        {
            if (OnChatMessage != null)
                OnChatMessage(this, e);
        }

        #endregion
	}
}

