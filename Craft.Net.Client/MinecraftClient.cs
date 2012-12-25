using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Craft.Net.Client.Handlers;

namespace Craft.Net.Client
{
    public partial class MinecraftClient
    {
        static MinecraftClient()
        {
            PacketHandlerDelegates = new PacketHandler[256];
            PacketHandlers.RegisterHandlers();
        }

        /// <summary>
        /// The protocol version supported by this client.
        /// </summary>
        public const int ProtocolVersion = 51;
        public const string FriendlyVersion = "1.4.6";

        public delegate void PacketHandler(MinecraftClient client, IPacket packet);
        private static PacketHandler[] PacketHandlerDelegates { get; set; }

        public static void RegisterPacketHandler(byte packetId, PacketHandler handler)
        {
            PacketHandlerDelegates[packetId] = handler;
        }

        public MinecraftClient(Session session)
        {
            Session = session;
            SendQueue = new ConcurrentQueue<IPacket>();
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
            var handshake = new HandshakePacket(PacketReader.ProtocolVersion, Session.Username,
                EndPoint.Address.ToString(), EndPoint.Port);
            SendPacket(handshake);
        }

        public void SendPacket(IPacket packet)
        {
            SendQueue.Enqueue(packet);
        }

        private void NetworkWorker()
        {
            while (true)
            {
                // Send queued packets
                while (SendQueue.Count != 0)
                {
                    IPacket packet;
                    if (SendQueue.TryDequeue(out packet))
                    {
                        try
                        {
                            // Write packet
                            packet.WritePacket(Stream);
#if DEBUG
                            LogProvider.Log(packet, true);
#endif
                            Stream.Flush();
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
#if DEBUG
                        LogProvider.Log(packet, false);
#endif
                        HandlePacket(packet);
                    }
                    catch { /* TODO */ }
                }
                Thread.Sleep(1);
            }
        }

        private void HandlePacket(IPacket packet)
        {
            if (PacketHandlerDelegates[packet.Id] != null)
                PacketHandlerDelegates[packet.Id](this, packet);
            LogProvider.Log("Warning: No packet handlers for 0x" + packet.Id.ToString("X2"), LogImportance.Low);
        }
    }
}
