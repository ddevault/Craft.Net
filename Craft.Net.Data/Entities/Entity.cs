using System;

namespace Craft.Net.Data.Entities
{
    public abstract class Entity
    {
        public Entity()
        {
            Fire = -20;
        }

        public int Id { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 OldPosition { get; set; } // TODO: Refactor away
        /// <summary>
        /// In meters per tick
        /// </summary>
        public Vector3 Velocity { get; set; }

        public int FallDistance { get; set; }
        // The Y location that falling began at
        internal double FallStart { get; set; }
        /// <summary>
        /// The number of ticks that remain before an entity
        /// on fire is put out. Negative values are indicitive
        /// of how long the entity may stand in a fire-creating
        /// block before catching fire.
        /// </summary>
        public int Fire { get; set; }
        public bool OnGround { get; set; }
        public Dimension Dimension { get; set; }

        private float _Pitch;
        public float OldPitch { get; set; }
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
        public float OldYaw { get; set; }
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