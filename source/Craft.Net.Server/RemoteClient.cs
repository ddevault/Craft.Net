using Craft.Net.Common;
using Craft.Net.Networking;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Craft.Net.Server
{
    public class RemoteClient
    {
        public RemoteClient(TcpClient client)
        {
            NetworkClient = client;
            PacketQueue = new ConcurrentQueue<IPacket>();
        }

        public TcpClient NetworkClient { get; set; }
        public MinecraftStream NetworkStream { get; set; }
        public bool IsLoggedIn { get; set; }
        public ConcurrentQueue<IPacket> PacketQueue { get; set; }
        public bool EncryptionEnabled { get; protected internal set; }

        protected internal byte[] SharedKey { get; set; }

        public void SendPacket(IPacket packet)
        {
            PacketQueue.Enqueue(packet);
        }

        public void Disconnect(string reason)
        {
            SendPacket(new DisconnectPacket(reason));
        }
    }
}
