using System;
using System.Threading;
using Craft.Net.Anvil;
using Craft.Net.Common;
using Craft.Net.Networking;
using Craft.Net.Logic;
using Craft.Net.Physics;

namespace Craft.Net.Client
{
    public partial class MinecraftClient
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
            get { return _onGround; }
            private set { _onGround = value; }
        }

        public Vector3 Position
        {
            get
            {
                Vector3 ret;
                // Make sure that position data is in a consistent state.
                lock (_positionLock)
                {
                    ret = _position;
                }
                return ret;
            }
            set
            {
                lock (_positionLock)
                {
                    _positionChanged = _position != value;
                    _position = value;
                }
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

