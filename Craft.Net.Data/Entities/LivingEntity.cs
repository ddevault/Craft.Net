using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

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

        private Timer deathTimer { get; set; }

        /// <summary>
        /// Starts the death timer for despawn. For servers, use
        /// EntityManager to kill entities by setting the entity
        /// health to zero.
        /// </summary>
        public void Kill()
        {
            deathTimer.Change(3000, Timeout.Infinite);
        }

        public event EventHandler DeathAnimationComplete;

        private void OnDeathHandlerComplete(object discarded)
        {
            if (DeathAnimationComplete != null)
                DeathAnimationComplete(this, null);
        }

        // TODO: Potion effects
        // TODO: Equipment
    }
}
