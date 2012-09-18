using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using Craft.Net.Data;
using Craft.Net.Data.Entities;
using Craft.Net.Data.Events;
using Craft.Net.Data.Items;
using Craft.Net.Data.Windows;
using Craft.Net.Server.Events;
using Craft.Net.Server.Packets;

namespace Craft.Net.Server
{
    /// <summary>
    /// Manages transmission of entities from client to client
    /// </summary>
    public class EntityManager
    {
        protected internal static int nextEntityId = 1;
        protected internal MinecraftServer server;

        public EntityManager(MinecraftServer server)
        {
            this.server = server;
        }

        #region Public entity management

        public void SpawnEntity(World world, Entity entity)
        {
            entity.Id = nextEntityId++;
            world.Entities.Add(entity);
            // Get nearby clients in the same world
            var clients = GetClientsInWorld(world)
                .Where(c => !c.IsDisconnected && c.Entity.Position.DistanceTo(entity.Position) < (c.ViewDistance * Chunk.Width));
            entity.PropertyChanged += EntityOnPropertyChanged;

            if (entity is LivingEntity)
                (entity as LivingEntity).EntityDamaged += EntityDamaged;

            if (clients.Count() != 0)
            {
                // Spawn entity on relevant clients
                if (entity is PlayerEntity)
                {
                    // Isolate the client being spawned
                    (entity as PlayerEntity).Abilities.PropertyChanged += PlayerAbilitiesChanged; 
                    (entity as PlayerEntity).Inventory.WindowChange += PlayerInventoryChange; 
                    var client = clients.First(c => c.Entity == entity);
                    client.Entity.BedStateChanged += EntityOnUpdateBedState;
                    client.Entity.BedTimerExpired += EntityOnBedTimerExpired;
                    client.Entity.StartEating += PlayerStartEating;
                    clients = clients.Where(c => c.Entity != entity);
                    clients.ToList().ForEach(c => {
                        c.SendPacket(new SpawnNamedEntityPacket(client));
                        c.SendPacket(new EntityHeadLookPacket(client.Entity));
                        for (int i = 0; i < 4; i++)
                        {
                            var item = client.Entity.Inventory[InventoryWindow.ArmorIndex + i];
                            if (!item.Empty)
                                c.SendPacket(new EntityEquipmentPacket(client.Entity.Id,
                                    (EntityEquipmentSlot)(4 - i), item));
                        }
                        c.KnownEntities.Add(client.Entity.Id);
                    });
                }
                else if (entity is ItemEntity)
                {
                    clients.ToList().ForEach(c =>
                    {
                        c.SendPacket(new SpawnDroppedItemPacket(entity as ItemEntity));
                        c.KnownEntities.Add(entity.Id);
                    });
                }
                else if (entity is ObjectEntity)
                {
                    clients.ToList().ForEach(c =>
                    {
                        c.SendPacket(new SpawnObjectPacket(entity as ObjectEntity));
                        c.KnownEntities.Add(entity.Id);
                    });
                }
            }
            server.ProcessSendQueue();
        }

        public void TeleportEntity(Entity entity, Vector3 position)
        {
            var clients = GetKnownClients(entity);
            if (entity is PlayerEntity)
                clients = clients.Concat(new MinecraftClient[] { GetClient(entity as PlayerEntity) });
            entity.Position = position;
            foreach (var client in clients)
                client.SendPacket(new EntityTeleportPacket(entity));
        }

        public void DespawnEntity(Entity entity)
        {
            DespawnEntity(GetEntityWorld(entity), entity);
        }

        public void DespawnEntity(World world, Entity entity)
        {
            if (world == null)
                return;
            if (!world.Entities.Contains(entity))
                return;
            entity.PropertyChanged -= EntityOnPropertyChanged;
            world.Entities.Remove(entity);
            var clients = GetClientsInWorld(world).Where(c => c.KnownEntities.Contains(entity.Id));
            foreach (var client in clients)
            {
                client.KnownEntities.Remove(entity.Id);
                client.SendPacket(new DestroyEntityPacket(entity.Id));
            }
            server.ProcessSendQueue();
        }

        /// <summary>
        /// Note: This will not correctly kill players. If you wish to kill
        /// a player, set its health to zero and EntityManager will automatically
        /// kill the player through the correct means.
        /// </summary>
        public void KillEntity(LivingEntity entity)
        {
            entity.DeathAnimationComplete += (sender, args) =>
            {
                if (entity.Health <= 0)
                    DespawnEntity(entity);
            };
            if (entity is PlayerEntity)
            {
                var player = entity as PlayerEntity;
                server.OnPlayerDeath(new PlayerDeathEventArgs(player.LastDamageType, player, player.LastAttackingEntity));
            }
            entity.Kill();
            foreach (var client in GetKnownClients(entity))
                client.SendPacket(new EntityStatusPacket(entity.Id, EntityStatus.Dead));
        }

        /// <summary>
        /// Calculates which entities the client should be aware
        /// of, and sends them.
        /// </summary>
        public void SendClientEntities(MinecraftClient client)
        {
            var world = GetEntityWorld(client.Entity);
            var clients = GetClientsInWorld(world)
                .Where(c => !c.IsDisconnected && c.Entity.Position.DistanceTo(client.Entity.Position) <
                    (c.ViewDistance * Chunk.Width) && c != client);
            var entities = world.Entities.Where(e => !(e is PlayerEntity) && 
                e.Position.DistanceTo(client.Entity.Position) < (client.ViewDistance * Chunk.Width)); // TODO: Refactor into same thing
            foreach (var _client in clients)
            {
                client.KnownEntities.Add(_client.Entity.Id);
                client.SendPacket(new SpawnNamedEntityPacket(_client));
                client.SendPacket(new EntityEquipmentPacket(_client.Entity.Id, EntityEquipmentSlot.HeldItem, 
                    _client.Entity.Inventory[_client.Entity.SelectedSlot]));
                for (int i = 0; i < 4; i ++)
                {
                    var item = _client.Entity.Inventory[InventoryWindow.ArmorIndex + i];
                    if (!item.Empty)
                        client.SendPacket(new EntityEquipmentPacket(_client.Entity.Id, 
                            (EntityEquipmentSlot)(4 - i), item));
                }
            }
            foreach (var entity in entities)
            {
                client.KnownEntities.Add(entity.Id);
                if (entity is ItemEntity)
                    client.SendPacket(new SpawnDroppedItemPacket(entity as ItemEntity));
            }
        }

        #endregion

        private void EntityDamaged(object sender, EntityDamageEventArgs entityDamageEventArgs)
        {
            var entity = (LivingEntity)sender;
            var clients = GetKnownClients(entity);
            foreach (var minecraftClient in clients)
                minecraftClient.SendPacket(new EntityStatusPacket(entity.Id, EntityStatus.Hurt));
            server.ProcessSendQueue();
        }

        private void EntityOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            // Handles changes in entity properties
            var entity = sender as Entity;
            var clients = GetKnownClients(entity);
            if (entity is PlayerEntity)
            {
                var player = entity as PlayerEntity;
                var client = GetClient(player);
                switch (propertyChangedEventArgs.PropertyName)
                {
                    case "Health":
                    case "Food":
                    case "FoodSaturation":
                        client.SendPacket(new UpdateHealthPacket(player.Health, player.Food, player.FoodSaturation));
                        if (player.Health <= 0)
                            KillEntity(player);
                        return;
                    case "SpawnPoint":
                        client.SendPacket(new SpawnPositionPacket(player.SpawnPoint));
                        return;
                    case "GameMode":
                        client.SendPacket(new ChangeGameStatePacket(GameState.ChangeGameMode, client.Entity.GameMode));
                        client.SendPacket(new PlayerAbilitiesPacket(player.Abilities));
                        return;
                    case "Velocity":
                        client.SendPacket(new EntityVelocityPacket(player.Id, player.Velocity));
                        foreach (var knownClient in clients)
                            knownClient.SendPacket(new EntityVelocityPacket(player.Id, player.Velocity));
                        return;
                    case "GivenPosition":
                        UpdateGivenPosition(player);
                        return;
                }
            }
            switch (propertyChangedEventArgs.PropertyName)
            {
                case "Position":
                    UpdateEntityPosition(entity);
                    break;
                case "Pitch":
                case "Yaw":
                case "HeadLook":
                    UpdateEntityLook(entity);
                    break;
            }
        }

        private void EntityOnBedTimerExpired(object sender, EventArgs eventArgs)
        {
            var player = sender as PlayerEntity;
            var world = GetEntityWorld(player);
            var clients = GetClientsInWorld(world);
            foreach (var minecraftClient in clients)
            {
                if (minecraftClient.Entity.BedPosition == -Vector3.One)
                    return;
            }
            var level = server.GetLevel(world);
            level.Time = 0;
            foreach (var minecraftClient in clients)
            {
                minecraftClient.SendPacket(new AnimationPacket(minecraftClient.Entity.Id, Animation.LeaveBed));
                foreach (var client in GetKnownClients(minecraftClient.Entity))
                    client.SendPacket(new AnimationPacket(minecraftClient.Entity.Id, Animation.LeaveBed));
                minecraftClient.SendPacket(new TimeUpdatePacket(level.Time));
                minecraftClient.Entity.BedPosition = -Vector3.One;
            }
            server.ProcessSendQueue();
        }

        private void EntityOnUpdateBedState(object sender, EventArgs eventArgs)
        {
            var player = sender as PlayerEntity;
            var clients = GetKnownClients(player);
            if (player.BedPosition == -Vector3.One)
            {
                // Leave bed
                GetClient(player).SendPacket(new AnimationPacket(player.Id, Animation.LeaveBed));
                foreach (var minecraftClient in clients)
                    minecraftClient.SendPacket(new AnimationPacket(player.Id, Animation.LeaveBed));
            }
            else
            {
                if (server.GetLevel(GetEntityWorld(player)).Time % 24000 < 12000)
                {
                    GetClient(player).SendChat("You can only sleep at night.");
                    return;
                }
                // Enter bed
                GetClient(player).SendPacket(new UseBedPacket(player.Id, player.BedPosition));
                foreach (var minecraftClient in clients)
                    minecraftClient.SendPacket(new UseBedPacket(player.Id, player.BedPosition));
                player.SpawnPoint = player.BedPosition;
            }
            server.ProcessSendQueue();
        }

        private void PlayerAbilitiesChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "IsFlying")
                return;
            var entity = (PlayerEntity)sender;
            var client = GetClient(entity);
            client.SendPacket(new PlayerAbilitiesPacket(entity.Abilities));
        }

        private void PlayerInventoryChange(object sender, WindowChangeEventArgs windowChangeEventArgs)
        {
            var window = (Window)sender;
            if (windowChangeEventArgs.SlotIndex >= InventoryWindow.ArmorIndex &&
                windowChangeEventArgs.SlotIndex < InventoryWindow.ArmorIndex + 4)
            {
                // TODO: Prevent non-armor items from being used here
                var source = server.Clients.First(c => c.Entity.Inventory == window);
                var clients = GetKnownClients(source.Entity);
                foreach (var client in clients)
                {
                    int index = windowChangeEventArgs.SlotIndex - InventoryWindow.ArmorIndex;
                    EntityEquipmentSlot slot;
                    switch (index)
                    {
                        case 0:
                            slot = EntityEquipmentSlot.Footwear;
                            break;
                        case 1:
                            slot = EntityEquipmentSlot.Pants;
                            break;
                        case 2:
                            slot = EntityEquipmentSlot.Chestplate;
                            break;
                        default:
                            slot = EntityEquipmentSlot.Headgear;
                            break;
                    }
                    client.SendPacket(new EntityEquipmentPacket(source.Entity.Id, slot, windowChangeEventArgs.Value));
                }
            }
        }

        private void PlayerStartEating(object sender, EventArgs eventArgs)
        {
            var player = (PlayerEntity)sender;
            var client = GetClient(player);
            var slot = player.Inventory[player.SelectedSlot];
            slot.Index = player.SelectedSlot;
            var item = player.Inventory[player.SelectedSlot].Item as FoodItem;
            var known = GetKnownClients(player);
            foreach (var c in known)
                c.SendPacket(new AnimationPacket(client.Entity.Id, Animation.EatFood)); // TODO: Why doesn't this work
            var timer = new Timer(discarded =>
                {
                    if (player.SelectedSlot != slot.Index ||
                        player.SelectedItem.Empty || !(player.SelectedItem.Item is FoodItem))
                        return;
                    client.SendPacket(new EntityStatusPacket(player.Id, EntityStatus.EatingAccepted));
                    slot.Count--;
                    player.SetSlot(player.SelectedSlot, slot);
                    int food = player.Food;
                    food += item.FoodPoints;
                    if (food > 20) food = 0;
                    player.Food = (short)food;
                    float saturation = player.FoodSaturation;
                    saturation += item.Saturation;
                    if (saturation > food) saturation = food;
                    player.FoodSaturation = saturation;
                }, null, 1500, Timeout.Infinite);
        }

        private void UpdateGivenPosition(PlayerEntity entity)
        {
            // Used to update a player's position based on the one provided by
            // the client.
            entity.Position = entity.GivenPosition;
        }

        private void UpdateEntityPosition(Entity entity)
        {
            var clients = GetKnownClients(entity);
            if (entity is LivingEntity)
            {
                var world = GetEntityWorld(entity);
                var flooredPosition = entity.Position.Floor();

                // check for walked on blocks
                if (flooredPosition.Y == entity.Position.Y && entity.OldPosition.Floor() != entity.OldPosition)
                {
                    if ((flooredPosition + Vector3.Down).Y >= 0 && (flooredPosition + Vector3.Down).Y <= Chunk.Height)
                    {
                        var blockOn = world.GetBlock(flooredPosition + Vector3.Down);
                        blockOn.OnBlockWalkedOn(world, flooredPosition + Vector3.Down, entity);
                    }
                }
                if ((int)(entity.Position.X) != (int)(entity.OldPosition.X) ||
                    (int)(entity.Position.Y) != (int)(entity.OldPosition.Y) ||
                    (int)(entity.Position.Z) != (int)(entity.OldPosition.Z))
                {
                    if (flooredPosition.Y >= 0 && flooredPosition.Y <= Chunk.Height)
                    {
                        var blockIn = world.GetBlock(flooredPosition);
                        blockIn.OnBlockWalkedIn(world, flooredPosition, entity);
                    }
                }
            }
            foreach (var client in clients)
                client.SendPacket(new EntityTeleportPacket(entity));
        }

        private void UpdateEntityLook(Entity entity)
        {
            var clients = GetKnownClients(entity);
            foreach (var client in clients)
                client.SendPacket(new EntityHeadLookPacket(entity));
        }

        #region Utility methods

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
            if (!clients.Any())
                return null;
            return clients.First();
        }

        public World GetEntityWorld(Entity entity)
        {
            var firstOrDefault = server.Levels.FirstOrDefault(level => level.World.Entities.Contains(entity));
            if (firstOrDefault != null)
                return firstOrDefault.World;
            return server.DefaultWorld;
        }

        public Entity GetEntity(int id)
        {
            foreach (var level in server.Levels)
            {
                var entity = level.World.Entities.FirstOrDefault(e => e.Id == id);
                if (entity != null)
                    return entity;
            }
            return null;
        }

        #endregion
    }
}
