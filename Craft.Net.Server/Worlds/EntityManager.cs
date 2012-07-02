using System;
using System.Collections.Generic;

namespace Craft.Net.Server.Worlds
{
    public class EntityManager
    {
        public List<Entity> Entities;

        private int NextEntityId;

        public EntityManager()
        {
            NextEntityId = 0;
            Entities = new List<Entity>();
        }

        public void SpawnEntity(Entity Entity)
        {
            Entity.Id = NextEntityId++;
            Entities.Add(Entity);
            // TODO: Send to a bunch of clients
        }
    }
}

