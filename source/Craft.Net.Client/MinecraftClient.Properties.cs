using System;
using Craft.Net.Common;
using Craft.Net.Networking;

namespace Craft.Net.Client
{
    public partial class MinecraftClient
    {
        internal Vector3 _position;
        public Vector3 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                //SendPacket(new PlayerPositionPacket(Position.X, Position.Y, Position.Z, Position.Y + 1.62, true));
                SendPacket(new PlayerPositionAndLookPacket(Position.X, Position.Y, Position.Z, Position.Y + 1.62, Yaw, Pitch, true));
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

        public float Health { get; internal set; }
        public short Food { get; internal set; }
        public float FoodSaturation { get; internal set; }
    }
}

