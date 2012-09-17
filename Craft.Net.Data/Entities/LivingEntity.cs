using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Craft.Net.Data.Events;
using Craft.Net.Data.Windows;

namespace Craft.Net.Data.Entities
{
    /// <summary>
    /// Represents all mob and player entities
    /// </summary>
    public abstract class LivingEntity : Entity
    {
        public LivingEntity()
        {
            deathTimer = new Timer(OnDeathHandlerComplete, null, Timeout.Infinite, Timeout.Infinite);
        }

        private short air;
        private short health;

        public short Air
        {
            get { return air; }
            set
            {
                air = value;
                OnPropertyChanged("Air");
            }
        }

        public short Health
        {
            get { return health; }
            set
            {
                health = value;
                OnPropertyChanged("Health");
            }
        }

        public abstract short MaxHealth { get; }

        public virtual bool Invulnerable
        {
            get { return false; }
        }

        protected Timer deathTimer { get; set; }

        /// <summary>
        /// Starts the death timer for despawn. For servers, use
        /// EntityManager to kill entities by setting the entity
        /// health to zero.
        /// </summary>
        public virtual void Kill()
        {
            deathTimer.Change(3000, Timeout.Infinite);
        }

        public event EventHandler DeathAnimationComplete;

        private void OnDeathHandlerComplete(object discarded)
        {
            if (DeathAnimationComplete != null)
                DeathAnimationComplete(this, null);
        }

        public EventHandler<EntityDamageEventArgs> EntityDamaged;

        public virtual void Damage(int damage, bool accountForArmor = true)
        {
            // TODO: Armor
            if (Invulnerable)
                return;
            if (accountForArmor)
            {
                var player = (PlayerEntity)this; // TODO: Fix for different kinds of mobs
                double armorValue = 0;
                for (int i = 0; i < 4; i++)
                {
                    var slot = player.Inventory[InventoryWindow.ArmorIndex + i];
                    if (!slot.Empty)
                    {
                        var item = slot.Item as IArmorItem;
                        if (item == null)
                            continue;
                        armorValue += (item.ArmorBonus * 0.04);
                    }
                }
                damage = (int)(damage * (1 - armorValue));
            }
            Health -= (short)damage;
            if (EntityDamaged != null)
                EntityDamaged(this, new EntityDamageEventArgs(damage, Health));
        }

        // TODO: Potion effects
        // TODO: Equipment
    }
}
