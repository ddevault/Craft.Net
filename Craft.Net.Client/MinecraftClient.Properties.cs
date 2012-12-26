using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Craft.Net.Data;

namespace Craft.Net.Client
{
    public partial class MinecraftClient
    {
        public Session Session { get; set; }
        public TcpClient Client { get; set; }
        public ConcurrentQueue<IPacket> SendQueue { get; set; }
        public IPEndPoint EndPoint { get; set; }

        // TODO: Move to entity object
        #region Position and Look

        internal bool InitialPositionRecieved { get; set; }

        internal Vector3 _position;
        public Vector3 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                SendPacket(new PlayerPositionPacket(
                    Position.X, Position.Y, Position.Z, Position.Y + 1.62, true));
            }
        }

        internal float _pitch;
        public float Pitch
        {
            get { return _pitch;  }
            set
            {
                _pitch = value;
                SendPacket(new PlayerLookPacket(Yaw, Pitch, true));
            }
        }

        internal float _yaw;
        public float Yaw
        {
            get { return _yaw; }
            set
            {
                _yaw = value;
                SendPacket(new PlayerLookPacket(Yaw, Pitch, true));
            }
        }

        #endregion

        public bool Spawned { get; internal set; }
        public int EntityId { get; internal set; }

        protected internal MinecraftStream Stream { get; set; }
        protected internal NetworkStream NetworkStream { get; set; }

        internal byte[] SharedSecret { get; set; }

        private Thread NetworkWorkerThread { get; set; }
    }
}
