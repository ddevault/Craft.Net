using Craft.Net.Anvil;
using Craft.Net.Common;
using Craft.Net.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Craft.Net.Server
{
    public class MinecraftServer
    {
        public delegate void PacketHandler(RemoteClient client, MinecraftServer server, IPacket packet);

        #region Constructors

        public MinecraftServer()
        {
            PacketHandlers = new PacketHandler[256];
            Handlers.PacketHandlers.RegisterHandlers(this);
            NetworkLock = new object();
            NetworkThread = new Thread(NetworkWorker);
            Clients = new List<RemoteClient>();
            Settings = ServerSettings.DefaultSettings;
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

        protected internal RSACryptoServiceProvider CryptoServiceProvider { get; set; }
        protected internal RSAParameters ServerKey { get; set; }

        protected Thread NetworkThread { get; set; }
        protected PacketHandler[] PacketHandlers { get; set; }

        private object NetworkLock { get; set; }
        private DateTime NextKeepAlive { get; set; }
        private DateTime NextPlayerUpdate { get; set; }

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

            CryptoServiceProvider = new RSACryptoServiceProvider(1024);
            ServerKey = CryptoServiceProvider.ExportParameters(true);

            StartTime = DateTime.Now;

            Listener = new TcpListener(endPoint);
            Listener.Start();
            Listener.BeginAcceptTcpClient(AcceptClientAsync, null);

            NetworkThread.Start();
        }

        #endregion

        #region Protected methods

        protected internal void AcceptClientAsync(IAsyncResult result)
        {
            var client = new RemoteClient(Listener.EndAcceptTcpClient(result));
            client.NetworkStream = new MinecraftStream(new BufferedStream(client.NetworkClient.GetStream()));
            // TODO: Ban players
            lock (NetworkLock)
                Clients.Add(client);
            Listener.BeginAcceptTcpClient(AcceptClientAsync, null);
        }

        #endregion

        #region Private methods

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
                                        new AesStream(client.NetworkStream, client.SharedKey)));
                                    client.EncryptionEnabled = true;
                                }
                            }
                        }
                        if (disconnect)
                        {
                            Clients.RemoveAt(i--);
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
                                    Clients.RemoveAt(i--);
                                    break;
                                }
                                HandlePacket(client, packet);
                            }
                            catch (SocketException e)
                            {
                                Clients.RemoveAt(i--);
                                break;
                            }
                            catch (InvalidOperationException e)
                            {
                                new DisconnectPacket(e.Message).WritePacket(client.NetworkStream);
                                client.NetworkStream.Flush();
                                Clients.RemoveAt(i--);
                                break;
                            }
                            catch (Exception e)
                            {
                                new DisconnectPacket(e.Message).WritePacket(client.NetworkStream);
                                client.NetworkStream.Flush();
                                Clients.RemoveAt(i--);
                                break;
                            }
                        }
                    }
                }
                Thread.Sleep(10);
            }
        }

        private void HandlePacket(RemoteClient client, IPacket packet)
        {
            if (PacketHandlers[packet.Id] == null)
                throw new InvalidOperationException("No packet handler registered for 0x" + packet.Id.ToString("X2"));
            PacketHandlers[packet.Id](client, this, packet);
        }

        private void UpdateScheduledEvents()
        {
            if (DateTime.Now > NextKeepAlive)
            {
                NextKeepAlive = DateTime.Now.AddSeconds(30);
            }
            if (DateTime.Now > NextPlayerUpdate)
            {
                NextPlayerUpdate = DateTime.Now.AddMinutes(1);
            }
        }

        #endregion
    }
}
