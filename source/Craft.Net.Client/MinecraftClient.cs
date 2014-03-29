using System;
using Craft.Net.Networking;
using Craft.Net.Physics;
using Craft.Net.Logic;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using Craft.Net.Common;
using System.Threading;
using Craft.Net.Client.Events;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Craft.Net.Client
{
    public partial class MinecraftClient
    {
        public delegate void PacketHandler(MinecraftClient client, IPacket packet);

        public MinecraftClient(Session session)
        {
            Session = session;
            PacketQueue = new ConcurrentQueue<IPacket>();
            PacketHandlers = new Dictionary<Type, PacketHandler>();
            Handlers.PacketHandlers.Register(this);
        }

        public Session Session { get; set; }
        public TcpClient Client { get; set; }
        public ConcurrentQueue<IPacket> PacketQueue { get; set; }
        public IPEndPoint EndPoint { get; set; }

        public ReadOnlyWorld World { get; protected internal set; }
        public int EntityId { get; protected internal set; }

        protected internal NetworkStream NetworkStream { get; set; }
        protected internal NetworkManager NetworkManager { get; set; }

        internal byte[] SharedSecret { get; set; }
        internal bool IsLoggedIn { get; set; }
        internal bool IsSpawned { get; set; }

        private Thread NetworkWorkerThread { get; set; }
        private Dictionary<Type, PacketHandler> PacketHandlers { get; set; }
        private ManualResetEvent NetworkingReset { get; set; }

        public void RegisterPacketHandler(Type packetType, PacketHandler handler)
        {
            if (!packetType.GetInterfaces().Any(p => p == typeof(IPacket)))
                throw new InvalidOperationException("Packet type must implement Craft.Net.Networking.IPacket");
            PacketHandlers[packetType] = handler;
        }

        public void Connect(IPEndPoint endPoint)
        {
            if (Client != null && Client.Connected)
                throw new InvalidOperationException("Already connected to a server!");
            EndPoint = endPoint;
            Client = new TcpClient();
            Client.Connect(EndPoint);
            NetworkStream = Client.GetStream();
            NetworkManager = new NetworkManager(NetworkStream);
            NetworkingReset = new ManualResetEvent(true);
            NetworkWorkerThread = new Thread(NetworkWorker);
            PhysicsWorkerThread = new Thread(PhysicsWorker);

            NetworkWorkerThread.Start();
            var handshake = new HandshakePacket(NetworkManager.ProtocolVersion, 
                EndPoint.Address.ToString(), (ushort)EndPoint.Port, NetworkMode.Login);
            SendPacket(handshake);
            var login = new LoginStartPacket(Session.SelectedProfile.Name);
            SendPacket(login);
            PhysicsWorkerThread.Start();
        }

        public void Disconnect(string reason)
        {
            NetworkWorkerThread.Abort();
            if (Client.Connected)
            {
                try
                {
                    NetworkManager.WritePacket(new DisconnectPacket(reason), PacketDirection.Serverbound);
                    Client.Close();
                }
                catch { }
            }
        }

        public void Respawn()
        {
            if (Health > 0)
                throw new InvalidOperationException("Player is not dead!");
            SendPacket(new ClientStatusPacket(ClientStatusPacket.StatusChange.Respawn));
        }

        public void SendPacket(IPacket packet)
        {
            PacketQueue.Enqueue(packet);
        }

        public void SendChat(string message)
        {
            SendPacket(new ChatMessagePacket(message));
        }

        private DateTime nextPhysicsUpdate = DateTime.MinValue;
        private Thread PhysicsWorkerThread;
        private PhysicsEngine engine;
        private void PhysicsWorker()
        {
            while (NetworkWorkerThread.IsAlive)
            {
                if (nextPhysicsUpdate < DateTime.Now)
                {
                    //We need to wait for a login packet to initialize the physics subsystem
                    if (World != null && engine == null)
                    {
                        // 50 ms / update for 20 ticks per second
                        engine = new PhysicsEngine(World.World, Block.PhysicsProvider, 50);
                        engine.AddEntity(this);
                    }
                    nextPhysicsUpdate = DateTime.Now.AddMilliseconds(50);
                    try
                    {
                        engine.Update();
                    }
                    catch (Exception)
                    {
                        // Sometimes the world hasn't loaded yet, so the Phyics update can't properly read blocks and
                        // throws an exception.
                    }
                }
                else
                {
                    var sleepTime = (nextPhysicsUpdate - DateTime.Now).Milliseconds;
                    if (sleepTime > 0)
                    {
                        Thread.Sleep(sleepTime);
                    }
                }
            }
        }

        private DateTime nextPlayerUpdate = DateTime.MinValue;
        private void NetworkWorker()
        {
            while (true)
            {
                if (IsSpawned && nextPlayerUpdate < DateTime.Now)
                {
                    nextPlayerUpdate = DateTime.Now.AddMilliseconds(100);
                    lock (_positionLock)
                    {
                        SendPacket(new PlayerPacket(OnGround));

                        if (_positionChanged)
                        {
                            SendPacket(new PlayerPositionPacket(
                                Position.X,
                                Position.Y,
                                Position.Z,
                                Position.Y - 1.62,
                                OnGround
                            ));
                            _positionChanged = false;
                        }
                    }
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
                            NetworkManager.WritePacket(packet, PacketDirection.Serverbound);
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
                        var packet = NetworkManager.ReadPacket(PacketDirection.Clientbound);
                        HandlePacket(packet);
                        if (packet is DisconnectPacket)
                        {
                            Console.WriteLine(((DisconnectPacket)packet).Reason);
                            return;
                        }
                    }
                    catch (Exception e) 
                    {
                         // TODO: OnNetworkException or something
                        Console.WriteLine(e);
                    }
                }
                NetworkingReset.Set();
                NetworkingReset.Reset();
                Thread.Sleep(1);
            }
        }

        protected internal void FlushPackets()
        {
            // Writes all pending packets to the underlying network stream
            NetworkingReset.WaitOne();
        }

        private void HandlePacket(IPacket packet)
        {
            var type = packet.GetType();
            if (PacketHandlers.ContainsKey(type))
                PacketHandlers[type](this, packet);
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