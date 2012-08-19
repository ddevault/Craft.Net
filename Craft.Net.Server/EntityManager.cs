using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Craft.Net.Data;
using Craft.Net.Data.Entities;
using Craft.Net.Server.Packets;

namespace Craft.Net.Server
{
    /// <summary>
    /// Manages transmission of entities from client to client
    /// </summary>
    public class EntityManager
    {
        private Queue<Entity> entitiesToDestroy;

        internal static int nextEntityId = 1;
        internal MinecraftServer server;

        public EntityManager(MinecraftServer server)
        {
            this.server = server;
            entitiesToDestroy = new Queue<Entity>();
        }

        public void SpawnEntity(World world, Entity entity)
        {
            entity.Id = nextEntityId++;
            world.Entities.Add(entity);
            // Get nearby clients in the same world
            var clients = GetClientsInWorld(world)
                .Where(c => !c.IsDisconnected && c.Entity.Position.DistanceTo(entity.Position) < (c.ViewDistance * Chunk.Width));

            if (clients.Count() != 0)
            {
                // Spawn entity on relevant clients
                if (entity is PlayerEntity)
                {
                    // Isolate the client being spawned
                    var client = clients.Where(c => c.Entity == entity).First();
                    clients = clients.Where(c => c.Entity != entity);
                    clients.ToList().ForEach(c => {
                        c.SendPacket(new SpawnNamedEntityPacket(client));
                        c.KnownEntities.Add(client.Entity.Id);
                    });
                }
            }
            server.ProcessSendQueue();
        }

        public void DespawnEntity(Entity entity)
        {
            DespawnEntity(GetEntityWorld(entity), entity);
        }

        public void DespawnEntity(World world, Entity entity)
        {
            world.Entities.Remove(entity);
            var clients = GetClientsInWorld(world).Where(c => c.KnownEntities.Contains(entity.Id));
            foreach (var client in clients)
            {
                client.KnownEntities.Remove(entity.Id);
                client.SendPacket(new DestroyEntityPacket(entity.Id));
            }
            server.ProcessSendQueue();
        }

        public IEnumerable<MinecraftClient> GetClientsInWorld(World world)
        {
            return server.Clients.Where(c => world.Entities.Contains(c.Entity));
        }

        public World GetEntityWorld(Entity entity)
        {
            foreach (var world in server.Worlds)
            {
                if (world.Entities.Contains(entity))
                    return world;
            }
            return null;
        }
    }
}
