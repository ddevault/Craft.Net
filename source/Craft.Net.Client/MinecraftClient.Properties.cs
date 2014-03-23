using System;
using Craft.Net.Anvil;
using Craft.Net.Common;
using Craft.Net.Networking;
using Craft.Net.Logic;

namespace Craft.Net.Client
{
    public partial class MinecraftClient
    {
        internal Vector3 _position;
        bool _onGround;
        public Vector3 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                var coordinates = new Coordinates3D((int)_position.X, (int)_position.Y, (int)_position.Z);
                var blockBoundingBox = LogicManager.GetBoundingBox(World.GetBlockId(coordinates)).Value;
                if (blockBoundingBox.Contains(_position - (Vector3)coordinates))
                    _onGround = true;
                else
                    _onGround = false;
                SendPacket(new PlayerPositionPacket(Position.X, Position.Y, Position.Z, Position.Y - 1.62, _onGround));
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

