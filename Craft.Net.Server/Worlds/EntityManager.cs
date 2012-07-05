using System;
using System.Collections.Generic;
using Craft.Net.Server.Worlds.Entities;
using Craft.Net.Server.Packets;

namespace Craft.Net.Server.Worlds
{
    public class EntityManager
    {
        public List<Entity> Entities;
        public World World;
        public MinecraftServer Server;

        private static int NextEntityId;

        public EntityManager(World World)
        {
            NextEntityId = 0;
            Entities = new List<Entity>();
            this.World = World;
        }

        public void SpawnEntity(Entity Entity)
        {
            Entity.Id = NextEntityId++;
            Entities.Add(Entity);
            if (Entity is PlayerEntity)
            {
                PlayerEntity player = (PlayerEntity)Entity;
                foreach (var client in Server.GetClientsInWorld(World))
                {
                    if (client != player.Client)
                        client.SendPacket(new SpawnNamedEntityPacket(player.Client));
                }
                Server.ProcessSendQueue();
            }
        }

        public void UpdateEntity(Entity entity)
        {

        }
    }
}

