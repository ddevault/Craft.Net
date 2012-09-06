using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Entities
{
    /// <summary>
    /// Represents all mob and player entities
    /// </summary>
    public abstract class LivingEntity : Entity
    {
        public short Air { get; set; }
        public short Health { get; set; }
        public abstract short MaxHealth { get; }
        // TODO: Death animation timer
        // TODO: Potion effects
        // TODO: Equipment
    }
}
