using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Craft.Net.Logic
{
    public class PlayerAbilities : INotifyPropertyChanged
    {
        public PlayerAbilities(PlayerEntity entity)
        {
            FirePropertyChanged = false;
            IsFlying = false;
            MayFly = false;
            Invulnerable = false;
            InstantMine = false;
            FirePropertyChanged = true;
            FlyingSpeed = 0.05f;
            WalkingSpeed = 0.1f;
            PlayerEntity = entity;
        }

        private bool isFlying;
        private bool mayFly;
        private bool invulnerable;
        private bool instantMine;
        private float flyingSpeed;
        private float walkingSpeed;

        public bool IsFlying
        {
            get { return isFlying; }
            set
            {
                isFlying = value;
                OnPropertyChanged("IsFlying");
            }
        }

        public bool MayFly
        {
            get { return mayFly; }
            set
            {
                mayFly = value;
                OnPropertyChanged("MayFly");
            }
        }

        public bool Invulnerable
        {
            get { return invulnerable; }
            set
            {
                invulnerable = value;
                OnPropertyChanged("Invulnerable");
            }
        }

        public bool InstantMine
        {
            get { return instantMine; }
            set
            {
                instantMine = value;
                OnPropertyChanged("InstantMine");
            }
        }

        public float FlyingSpeed
        {
            get { return flyingSpeed; }
            set
            {
                flyingSpeed = value;
                OnPropertyChanged("FlyingSpeed");
            }
        }

        public float WalkingSpeed
        {
            get { return walkingSpeed; }
            set
            {
                walkingSpeed = value;
                OnPropertyChanged("WalkingSpeed");
            }
        }

        private PlayerEntity PlayerEntity { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        internal bool FirePropertyChanged { get; set; }

        public void OnPropertyChanged(string propertyName)
        {
            if (!FirePropertyChanged)
                return;
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(PlayerEntity, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("IsFlying: " + IsFlying);
            sb.Append(", MayFly: " + MayFly);
            sb.Append(", Invulnerable: " + Invulnerable);
            sb.Append(", InstantMine: " + InstantMine);
            sb.Append(", WalkingSpeed: " + WalkingSpeed);
            sb.Append(", FlyingSpeed: " + FlyingSpeed);
            return sb.ToString();
        }

        public byte AsFlags()
        {
            return (byte)(
                        (Invulnerable ? 1 : 0) |
                        (IsFlying ? 2 : 0) |
                        (MayFly ? 4 : 0) |
                        (InstantMine ? 8 : 0)
                        );
        }
    }
}
