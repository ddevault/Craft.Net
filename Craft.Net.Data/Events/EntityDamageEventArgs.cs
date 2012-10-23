using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Events
{
    public class EntityDamageEventArgs : EventArgs
    {
        public int Damage { get; set; }
        public int Health { get; set; }

        public EntityDamageEventArgs(int damage, int health)
        {
            Damage = damage;
            Health = health;
        }
    }
}
