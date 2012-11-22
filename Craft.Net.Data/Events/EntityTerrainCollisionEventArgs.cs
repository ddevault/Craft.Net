using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Entities;

namespace Craft.Net.Data.Events
{
    public class EntityTerrainCollisionEventArgs : EventArgs
    {
        public Entity Entity { get; set; }
        public Vector3 Block { get; set; }
        public World World { get; set; }
        public Vector3 Direction { get; set; }
    }
}