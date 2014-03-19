using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Logic
{
    public abstract class ObjectEntity : Entity
    {
        public abstract byte EntityType { get; }
        public abstract int Data { get; }
    }
}
