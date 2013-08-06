using System;
using Craft.Net.Networking;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using Craft.Net.Common;
using System.Threading;
using Craft.Net.Client.Events;

namespace Craft.Net.Client
{
    public partial class MinecraftClient
    {
        public delegate void PacketHandler(MinecraftClient client, IPacket packet);

        public MinecraftClient(Session session)
        {
            Session = session;
            PacketQueue = new ConcurrentQueue<IPacket>();
            PacketHandlers = new PacketHandler[256];
            Handlers.PacketHandlers.Register(this);
        }

        public Session Session { get; set; }
        public TcpClient Client { get; set; }
        public ConcurrentQueue<IPacket> PacketQueue { get; set; }
        public IPEndPoint EndPoint { get; set; }

        public ReadOnlyWorld World { get; protected internal set; }
        public int EntityId { get; protected internal set; }

        protected internal MinecraftStream Stream { get; set; }
        protected internal NetworkStream NetworkStream { get; set; }

        internal byte[] SharedSecret { get; set; }
        internal bool IsLoggedIn { get; set; }
        internal bool IsSpawned { get; set; }

        private Thread NetworkWorkerThread { get; set; }
        private PacketHandler[] PacketHandlers { get; set; }

        public void RegisterPacketHandler(byte packetId, PacketHandler handler)
        {
            PacketHandlers[packetId] = handler;
        }

        public void Connect(IPEndPoint endPoint)
        {
            if (Client != null && Client.Connected)
                throw new InvalidOperationException("Already connected to a server!");
            EndPoint = endPoint;
            Client = new TcpClient();
            Client.Connect(EndPoint);
            NetworkStream = Client.GetStream();
            Stream = new MinecraftStream(new BufferedStream(NetworkStream));
            NetworkWorkerThread = new Thread(NetworkWorker);
            NetworkWorkerThread.Start();
            var handshake = new HandshakePacket(PacketReader.ProtocolVersion, Session.Username, EndPoint.Address.ToString(), EndPoint.Port);
            SendPacket(handshake);
        }

        public void Disconnect(string reason)
        {
            NetworkWorkerThread.Abort();
            if (Client.Connected)
            {
                try
                {
                    new DisconnectPacket(reason).WritePacket(Stream);
                    Stream.Flush();
                    Client.Close();
                }
                catch { }
            }
        }

        public void Respawn()
        {
            if (Health > 0)
                throw new InvalidOperationException("Player is not dead!");
            //SendPacket(new RespawnPacket(Dimension.Overworld, // TODO: Other dimensions
            //    Level.Difficulty, Level.GameMode, World.Height, Level.World.LevelType));
            SendPacket(new ClientStatusPacket(ClientStatusPacket.ClientStatus.Respawn));
        }

        public void SendPacket(IPacket packet)
        {
            PacketQueue.Enqueue(packet);
        }

        public void SendChat(string message)
        {
            SendPacket(new ChatMessagePacket(message));
        }

        private DateTime nextPlayerUpdate = DateTime.MinValue;
        private void NetworkWorker()
        {
            while (true)
            {
                if (IsSpawned && nextPlayerUpdate < DateTime.Now)
                {
                    nextPlayerUpdate = DateTime.Now.AddMilliseconds(500);
                    SendPacket(new PlayerPacket(true)); // TODO: Store OnGround properly
                }
                // Send queued packets
                while (PacketQueue.Count != 0)
                {
                    IPacket packet;
                    if (PacketQueue.TryDequeue(out packet))
                    {
                        try
                        {
                            // Write packet
                            packet.WritePacket(Stream);
                            Stream.Flush();
                            if (packet is DisconnectPacket)
                                return;
                        }
                        catch { /* TODO */ }
                    }
                }
                // Read incoming packets
                var readTimeout = DateTime.Now.AddMilliseconds(20); // Maximum read time given to server per iteration
                while (NetworkStream.DataAvailable && DateTime.Now < readTimeout)
                {
                    try
                    {
                        var packet = PacketReader.ReadPacket(Stream);
                        HandlePacket(packet);
                        if (packet is DisconnectPacket)
                            return;
                    }
                    catch { /* TODO */ }
                }
                Thread.Sleep(1);
            }
        }

        private void HandlePacket(IPacket packet)
        {
            if (PacketHandlers[packet.Id] != null)
                PacketHandlers[packet.Id](this, packet);
            //throw new InvalidOperationException("Recieved a packet we can't handle: " + packet.GetType().Name);
        }

        public event EventHandler<ChatMessageEventArgs> ChatMessage;
        protected internal virtual void OnChatMessage(ChatMessageEventArgs e)
        {
            if (ChatMessage != null) ChatMessage(this, e);
        }

        public event EventHandler LoggedIn;
        protected internal virtual void OnLoggedIn()
        {
            if (LoggedIn != null) LoggedIn(this, null);
        }

        public event EventHandler<DisconnectEventArgs> Disconnected;
        protected internal virtual void OnDisconnected(DisconnectEventArgs e)
        {
            if (Disconnected != null) Disconnected(this, e);
        }

        public event EventHandler<EntitySpawnEventArgs> InitialSpawn;
        protected internal virtual void OnInitialSpawn(EntitySpawnEventArgs e)
        {
            if (InitialSpawn != null) InitialSpawn(this, e);
        }

        public event EventHandler PlayerDied;
        protected internal virtual void OnPlayerDied()
        {
            if (PlayerDied != null) PlayerDied(this, null);
        }

        public event EventHandler<HealthAndFoodEventArgs> HealthOrFoodChanged;
        protected internal virtual void OnHealthOrFoodChanged(HealthAndFoodEventArgs e)
        {
            if (HealthOrFoodChanged != null) HealthOrFoodChanged(this, e);
        }
    }
}