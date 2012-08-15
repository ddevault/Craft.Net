using System.Collections.Generic;
using Craft.Net.Server.Packets;
using Craft.Net.Server.Worlds.Entities;

namespace Craft.Net.Server.Worlds
{
    public class EntityManager
    {
        private static int nextEntityId;
        public List<Entity> Entities;
        public MinecraftServer Server;
        public World World;

        public EntityManager(World world)
        {
            nextEntityId = 0;
            Entities = new List<Entity>();
            this.World = world;
        }

        public void SpawnEntity(Entity entity)
        {
            entity.Id = nextEntityId++;
            Entities.Add(entity);
            if (entity is PlayerEntity)
            {
                var player = (PlayerEntity)entity;
                foreach (MinecraftClient client in Server.GetClientsInWorld(World))
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