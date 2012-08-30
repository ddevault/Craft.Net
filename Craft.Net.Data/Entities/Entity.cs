using System;

namespace Craft.Net.Data.Entities
{
    public abstract class Entity
    {
        public Entity()
        {
            LastPositionUpdateTime = DateTime.Now;
            LastPositionUpdate = Position;
        }

        public Dimension Dimension;
        public int Id;
        public Vector3 OldPosition;
        public bool OnGround;
        public Vector3 Position;
        public DateTime LastPositionUpdateTime;
        public Vector3 LastPositionUpdate;
        public Vector3 Velocity;

        private float _Pitch;
        public float OldPitch;
        public float Pitch
        {
            get
            {
                return _Pitch;
            }
            set
            {
                OldPitch = _Pitch;
                _Pitch = value;
            }
        }

        private float _Yaw;
        public float OldYaw;
        public float Yaw
        {
            get
            {
                return _Yaw;
            }
            set
            {
                OldYaw = _Yaw;
                _Yaw = value;
            }
        }

        public abstract Size Size { get; }

        public virtual bool AffectedByGravity
        {
            get
            {
                return true;
            }
        }
    }
}