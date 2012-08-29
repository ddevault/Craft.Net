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

        public void SendClientEntities(MinecraftClient client)
        {
            var world = GetEntityWorld(client.Entity);
            var clients = GetClientsInWorld(world)
                .Where(c => !c.IsDisconnected && c.Entity.Position.DistanceTo(client.Entity.Position) < (c.ViewDistance * Chunk.Width) && c != client);
            foreach (var _client in clients)
            {
                client.KnownEntities.Add(_client.Entity.Id);
                client.SendPacket(new SpawnNamedEntityPacket(_client));
                client.SendPacket(new EntityEquipmentPacket(_client.Entity.Id, EntityEquipmentSlot.HeldItem, _client.Entity.Inventory[_client.Entity.SelectedSlot]));
            }
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

        public void UpdateEntity(Entity entity)
        {
            var world = GetEntityWorld(entity);

            if ((int)(entity.Position.X) != (int)(entity.OldPosition.X) ||
                (int)(entity.Position.Y) != (int)(entity.OldPosition.Y) ||
                (int)(entity.Position.Z) != (int)(entity.OldPosition.Z))
            {
                var flooredPosition = entity.Position.Floor();
                var blockIn = world.GetBlock(flooredPosition);
                var blockOn = world.GetBlock(flooredPosition + Vector3.Down);
                blockIn.OnBlockWalkedIn(world, flooredPosition, entity);
                if (flooredPosition.Y == entity.Position.Y)
                    blockOn.OnBlockWalkedOn(world, flooredPosition + Vector3.Down, entity);
            }

            // Update location with known clients
            if (entity.Position.DistanceTo(entity.OldPosition) > 0.1d ||
                entity.Pitch != entity.OldPitch || entity.Yaw != entity.OldYaw)
            {
                var knownClients = GetClientsInWorld(world).Where(c => c.KnownEntities.Contains(entity.Id));
                foreach (var client in knownClients)
                {
                    client.SendPacket(new EntityTeleportPacket(entity));
                    if (entity.Yaw != entity.OldYaw)
                        client.SendPacket(new EntityHeadLookPacket(entity));
                    // TODO: Further research into relative movement
                    // When relative movement packets are used, the remote
                    // clients inevitably see each other in a very inaccurate
                    // position.
                }
                server.ProcessSendQueue();
                entity.OldPosition = entity.Position;
            }
        }

        public IEnumerable<MinecraftClient> GetKnownClients(Entity entity)
        {
            return GetClientsInWorld(GetEntityWorld(entity)).Where(c => c.KnownEntities.Contains(entity.Id));
        }

        public IEnumerable<MinecraftClient> GetClientsInWorld(World world)
        {
            return server.Clients.Where(c => world.Entities.Contains(c.Entity));
        }

        public MinecraftClient GetClient(PlayerEntity entity)
        {
            var clients = server.Clients.Where(c => c.Entity == entity);
            if (clients.Count() == 0)
                return null;
            return clients.First();
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
