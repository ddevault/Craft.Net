using System.Collections.Generic;
using Craft.Net.Server.Packets;
using Craft.Net.Server.Worlds.Entities;

namespace Craft.Net.Server.Worlds
{
    public class EntityManager
    {
        private static int NextEntityId;
        public List<Entity> Entities;
        public MinecraftServer Server;
        public World World;

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
                var player = (PlayerEntity) Entity;
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