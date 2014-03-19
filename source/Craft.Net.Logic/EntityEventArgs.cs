using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Logic
{
    public class EntityEventArgs : EventArgs
    {
        public Entity Entity { get; set; }

        public EntityEventArgs(Entity entity)
        {
            Entity = entity;
        }
    }
}
