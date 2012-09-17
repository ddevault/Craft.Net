using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data;
using Craft.Net.Data.Entities;

namespace Craft.Net.Server.Events
{
    public class PlayerDeathEventArgs : EventArgs
    {
        public DamageType DeathType { get; set; }
        public Entity Killer { get; set; }
        public PlayerEntity Player { get; set; }
        public bool Handled { get; set; }

        public PlayerDeathEventArgs(DamageType deathType, PlayerEntity player)
        {
            DeathType = deathType;
            Handled = false;
            Player = player;
        }

        public PlayerDeathEventArgs(DamageType deathType, PlayerEntity player, Entity killer) : this(deathType, player)
        {
            Killer = killer;
        }
    }
}
