using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Events
{
    public class EntityDamageEventArgs : EventArgs
    {
        public int Damage { get; set; }
        public float Health { get; set; }

        public EntityDamageEventArgs(int damage, float health)
        {
            Damage = damage;
            Health = health;
        }
    }
}
