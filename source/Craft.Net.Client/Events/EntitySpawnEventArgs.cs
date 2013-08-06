using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Common;

namespace Craft.Net.Client.Events
{
    public class EntitySpawnEventArgs : EventArgs
    {
        public EntitySpawnEventArgs(Vector3 position, int entityId)
        {
            Position = position;
            EntityId = entityId;
        }

        // TODO: Use a Craft.Net.Data.Entity
        public Vector3 Position { get; set; }
        public int EntityId { get; set; }
    }
}