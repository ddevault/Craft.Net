using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Craft.Net.Client
{
    public partial class MinecraftClient
    {
        public Session Session { get; set; }
        public TcpClient Client { get; set; }
        public ConcurrentQueue<IPacket> SendQueue { get; set; }
        public IPEndPoint EndPoint { get; set; }

        protected internal MinecraftStream Stream { get; set; }
        protected internal NetworkStream NetworkStream { get; set; }

        internal byte[] SharedSecret { get; set; }

        private Thread NetworkWorkerThread { get; set; }
    }
}
