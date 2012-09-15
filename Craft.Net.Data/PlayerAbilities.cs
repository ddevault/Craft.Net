using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Craft.Net.Data.Entities;

namespace Craft.Net.Data
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
            PlayerEntity = entity;
        }

        private bool isFlying;
        private bool mayFly;
        private bool invulnerable;
        private bool instantMine;

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

        public byte FlyingSpeed { get; set; } // TODO
        public byte WalkingSpeed { get; set; } // TODO
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
    }
}
