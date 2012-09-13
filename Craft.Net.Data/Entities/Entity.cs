using System;
using System.ComponentModel;
using Craft.Net.Data.Metadata;

namespace Craft.Net.Data.Entities
{
    public abstract class Entity : INotifyPropertyChanged
    {
        protected Entity()
        {
            Fire = -20;
        }

        public int Id { get; set; }
        public Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }

        public Vector3 OldPosition { get; set; }
        /// <summary>
        /// In meters per tick
        /// </summary>
        public Vector3 Velocity
        {
            get { return velocity; }
            set
            {
                velocity = value;
                OnPropertyChanged("Velocity");
            }
        }

        public int FallDistance { get; set; }
        // The Y location that falling began at
        internal double FallStart { get; set; }
        /// <summary>
        /// The number of ticks that remain before an entity
        /// on fire is put out. Negative values are indicitive
        /// of how long the entity may stand in a fire-creating
        /// block before catching fire.
        /// </summary>
        public int Fire
        {
            get { return fire; }
            set
            {
                fire = value;
                OnPropertyChanged("Fire");
            }
        }

        public bool IsOnFire
        {
            get { return Fire > 0; }
        }

        public bool OnGround
        {
            get { return onGround; }
            set
            {
                onGround = value;
                OnPropertyChanged("OnGround");
            }
        }

        public Dimension Dimension
        {
            get { return dimension; }
            set
            {
                dimension = value;
                OnPropertyChanged("Dimension");
            }
        }

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
                OnPropertyChanged("Pitch");
            }
        }

        private float _Yaw;
        private Dimension dimension;
        private bool onGround;
        private int fire;
        private Vector3 position;
        private Vector3 velocity;
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
                OnPropertyChanged("Yaw");
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

        public virtual MetadataDictionary Metadata
        {
            get
            {
                var dictionary = new MetadataDictionary();
                dictionary[0] = new MetadataByte(0, 0); // Flags
                dictionary[8] = new MetadataInt(8, 0); // Potion effects
                return dictionary;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}