using System;
using Craft.Net.Anvil;
using Craft.Net.Common;
using Craft.Net.Networking;
using Craft.Net.Logic;

namespace Craft.Net.Client
{
    public partial class MinecraftClient
    {
        internal bool _onGround;
        public bool OnGround
        {
            get { return _onGround; }
            private set { _onGround = value; }
        }

        private void UpdateOnGround()
        {
            if (_position.Y > Craft.Net.Anvil.World.Height || _position.Y < 0)
            {
                _onGround = false;
            } else
            {
                try
                {
                    var feetPosition = new Vector3(_position.X, _position.Y - 1.62 - 1, _position.Z);
                    var coordinates = new Coordinates3D((int)feetPosition.X, (int)feetPosition.Y, (int)feetPosition.Z);
                    var blockBoundingBox = LogicManager.GetBoundingBox(World.GetBlockId(coordinates));
                    if (blockBoundingBox.HasValue && blockBoundingBox.Value.Contains(feetPosition - (Vector3)coordinates))
                        _onGround = true;
                    else
                        _onGround = false;
                } catch (ArgumentException)
                {
                    //Sometimes the world isn't loaded when we want it to be, so we pretend we are on the ground to
                    //prevent falling through the world
                    _onGround = true;
                }
            }
        }

        internal Vector3 _position;
        public Vector3 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                UpdateOnGround();
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

