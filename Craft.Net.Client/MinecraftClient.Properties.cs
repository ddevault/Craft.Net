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

        internal Vector3 _position;
        public Vector3 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                SendPacket(new PlayerPositionPacket(
                    Position.X, Position.Y, Position.Z, Position.Y + 1.62, false));
            }
        }

        internal float _pitch;
        public float Pitch
        {
            get { return _pitch;  }
            set
            {
                _pitch = value;
                SendPacket(new PlayerLookPacket(Yaw, Pitch, false));
            }
        }

        internal float _yaw;
        public float Yaw
        {
            get { return _yaw; }
            set
            {
                _yaw = value;
                SendPacket(new PlayerLookPacket(Yaw, Pitch, false));
            }
        }

        #endregion

        public float Health { get; internal set; }
        public short Food { get; internal set; }
        public float FoodSaturation { get; internal set; }

        public LevelInformation LevelInformation { get; internal set; }
        public ReadOnlyWorld World { get; internal set; }

        public bool IsSpawned { get; internal set; }
        public bool IsLoggedIn { get; internal set; }
        public int EntityId { get; internal set; }

        protected internal MinecraftStream Stream { get; set; }
        protected internal NetworkStream NetworkStream { get; set; }

        internal byte[] SharedSecret { get; set; }

        private Thread NetworkWorkerThread { get; set; }
    }
}
