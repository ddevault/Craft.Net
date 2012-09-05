using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Entities
{
    public abstract class LivingEntity : Entity
    {
        public short Air { get; set; }
        public short Health { get; set; }
        public abstract short MaxHealth { get; }
    }
}
