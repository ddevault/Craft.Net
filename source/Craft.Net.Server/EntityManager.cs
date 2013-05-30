using Craft.Net.Anvil;
using Craft.Net.Common;
using Craft.Net.Entities;
using Craft.Net.Physics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Server
{
    /// <summary>
    /// Manages entity chunk assignment and keeps track of which clients may see which entities.
    /// </summary>
    public class EntityManager
    {
        public EntityManager(MinecraftServer server)
        {
            NextEntityId = 1;
            Server = server;
            Entities = new List<Entity>();
            MarkedForDespawn = new ConcurrentQueue<int>();
        }

        public MinecraftServer Server { get; set; }
        public int NextEntityId { get; private set; }
        public List<Entity> Entities { get; set; }
        public static int MaxClientDistance = 4;
        public ConcurrentQueue<int> MarkedForDespawn { get; set; }

        public void SpawnEntity(World world, Entity entity)
        {
            entity.EntityId = NextEntityId++;
            entity.World = world;
            if (entity is IDiskEntity) // Assign chunk
            {
                var chunk = GetEntityChunk(entity.World, entity.Position);
                chunk.Entities.Add((IDiskEntity)entity);
                chunk.IsModified = true;
            }
            entity.PropertyChanged -= EntityPropertyChanged;
            entity.PropertyChanged += EntityPropertyChanged;
            Entities.Add(entity);
            SpawnOnClients(entity);
            entity.Despawn -= EntityDespawn;
            entity.Despawn += EntityDespawn;
            if (entity is IPhysicsEntity)
            {
                // Add to physics engine
                var engine = Server.GetPhysicsForWorld(world);
                engine.AddEntity((IPhysicsEntity)entity);
            }
        }

        void EntityDespawn(object sender, EventArgs e)
        {
            Despawn(sender as Entity);
        }

        public void Despawn(Entity entity)
        {
            MarkedForDespawn.Enqueue(entity.EntityId);
        }

        public RemoteClient[] GetKnownClients(Entity entity)
        {
            return Server.Clients.Where(c => c.KnownEntities.Contains(entity.EntityId)).ToArray();
        }

        public Entity[] GetEntitiesInRange(Entity entity, int maxChunks)
        {
            return Entities.Where(e => e != entity && IsInRange(e.Position, entity.Position, maxChunks)).ToArray();
        }

        public void SendClientEntities(RemoteClient client)
        {
            foreach (var entity in GetEntitiesInRange(client.Entity, MaxClientDistance))
                client.TrackEntity(entity);
        }

        public void Update()
        {
            lock (Entities)
            {
                foreach (var entity in Entities)
                    entity.Update(GetEntitiesInRange(entity, 2));
                while (MarkedForDespawn.Count != 0)
                {
                    int id;
                    while (!MarkedForDespawn.TryDequeue(out id));
                    var entity = GetEntityById(id);
                    Entities.Remove(entity);
                    if (entity is IPhysicsEntity)
                    {
                        // Remove from physics engine
                        var engine = Server.GetPhysicsForWorld(entity.World);
                        engine.RemoveEntity((IPhysicsEntity)entity);
                    }
                    foreach (var client in GetKnownClients(entity))
                        client.ForgetEntity(entity);
                    entity.PropertyChanged -= EntityPropertyChanged;
                }
            }
        }

        private Entity GetEntityById(int id)
        {
            return Entities.FirstOrDefault(e => e.EntityId == id);
        }

        private Chunk GetEntityChunk(World world, Vector3 position)
        {
            return world.FindChunk(position);
        }

        private void SpawnOnClients(Entity entity)
        {
            var clients = Server.Clients.Where(c => c.IsLoggedIn && !c.KnownEntities.Contains(entity.EntityId)
                        && IsInRange(c.Entity.Position, entity.Position, MaxClientDistance)).ToArray();
            foreach (var client in clients)
                client.TrackEntity(entity);
        }

        private void EntityPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var entity = sender as Entity;
            var diskEntity = sender as IDiskEntity;
            if (e.PropertyName == "Position")
            {
                if ((int)(entity.Position.X) >> 4 != (int)(entity.OldPosition.X) >> 4 ||
                    (int)(entity.Position.Z) >> 4 != (int)(entity.OldPosition.Z) >> 4)
                {
                    // Handle moving between chunks
                    if (diskEntity != null)
                    {
                        var oldChunk = GetEntityChunk(entity.World, entity.OldPosition);
                        if (oldChunk.Entities.Contains(diskEntity))
                            oldChunk.Entities.Remove(diskEntity);
                        oldChunk.IsModified = true;

                        var chunk = GetEntityChunk(entity.World, entity.Position);
                        chunk.Entities.Add(diskEntity);
                        chunk.IsModified = true;
                    }
                    var oldClients = GetKnownClients(entity).Where(c => c.IsLoggedIn && c.KnownEntities.Contains(entity.EntityId)
                        && !IsInRange(c.Entity.Position, entity.Position, MaxClientDistance)).ToArray();
                    var newClients = Server.Clients.Where(c => c.Entity != entity && c.IsLoggedIn && !c.KnownEntities.Contains(entity.EntityId)
                        && IsInRange(c.Entity.Position, entity.Position, MaxClientDistance)).ToArray();
                    foreach (var client in oldClients)
                        client.ForgetEntity(entity);
                    foreach (var client in newClients)
                        client.TrackEntity(entity);
                }
            }
            if (e.PropertyName == "Position" || e.PropertyName == "Yaw" || e.PropertyName == "Pitch" || e.PropertyName == "HeadYaw")
            {
                lock (Server.NetworkLock)
                {
                    foreach (var client in Server.Clients.Where(c => c.KnownEntities.Contains(entity.EntityId)))
                    {
                        client.UpdateEntity(entity);
                    }
                }
            }
        }

        private bool IsInRange(Vector3 a, Vector3 b, int range)
        {
            return Math.Abs(a.X - b.X) < range * Chunk.Width &&
                Math.Abs(a.Z - b.Z) < range * Chunk.Depth;
        }
    }
}
