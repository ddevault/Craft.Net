using System;
using System.Threading;
using Craft.Net.Anvil;
using Craft.Net.Common;
using Craft.Net.Networking;
using Craft.Net.Logic;
using Craft.Net.Physics;

namespace Craft.Net.Client
{
    public partial class MinecraftClient : IPhysicsEntity
    {
        // Private so that silly people won't try to modify it behind our backs - everything should go through the
        // Position and OnGround properties

        // Lock on all position data
        private object _positionLock = new object();
        private bool _onGround;
        private Vector3 _position;
        // We keep track of when the position has been set so that the NetworkWorker can update it properly.
        private bool _positionChanged;

        public bool OnGround
        {
            get
            {
                // Make sure that all position data is in a consistant state.
                lock (_positionLock) {
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
                            _onGround = blockBoundingBox.HasValue && blockBoundingBox.Value.Contains(feetPosition - (Vector3)coordinates);
                        } catch (ArgumentException)
                        {
                            //Sometimes the world isn't loaded when we want it to be, so we pretend we are on the ground to
                            //prevent falling through the world
                            _onGround = true;
                        }
                    }
                }
                return _onGround;
            }
            private set { _onGround = value; }
        }

        public Vector3 Position
        {
            get
            {
                Vector3 ret;
                // Make sure that position data is in a consistent state.
                lock(_positionLock) {
                    ret = _position;
                }
                return ret;
            }
            set
            {
                // Should be safe since C# locks are reentrant
                // This means that even if the thread has already locked the _positionLock (e.g. the physics thread
                // does this) we are safe.
                lock(_positionLock) {
                    _position = value;
                }
            }
        }

        // The physics engine needs a few properties as well
        private object _velocityLock = new object();
        private Vector3 _velocity = new Vector3();
        public Vector3 Velocity
        {
            get
            {
                Vector3 ret;
                lock (_velocityLock)
                {
                    ret = _velocity;
                }
                return ret;
            }

            set
            {
                lock (_velocityLock)
                {
                    _velocity = value;
                }
            }
        }

        public float AccelerationDueToGravity
        {
            get { return 32.0f;}
        }

        public float Drag
        {
            get { return 0.1f;}
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

