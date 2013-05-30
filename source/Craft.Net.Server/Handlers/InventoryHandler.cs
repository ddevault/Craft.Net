using Craft.Net.Common;
using Craft.Net.Entities;
using Craft.Net.Logic.Items;
using Craft.Net.Logic.Windows;
using Craft.Net.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Handlers
{
    internal class InventoryHandlers
    {
        public static void CreativeInventoryAction(RemoteClient client, MinecraftServer server, IPacket _packet)
        {
            var packet = (CreativeInventoryActionPacket)_packet;
            if (packet.SlotIndex == -1)
            {
                var entity = new ItemEntity(client.Entity.Position +
                                new Vector3(0, client.Entity.Size.Height, 0), packet.Item);
                entity.Velocity = MathHelper.FowardVector(client.Entity.Yaw) * new Vector3(0.25);
                server.EntityManager.SpawnEntity(client.Entity.World, entity);
            }
            else if (packet.SlotIndex < client.Entity.Inventory.Length && packet.SlotIndex > 0)
            {
                client.Entity.Inventory[packet.SlotIndex] = packet.Item;
                if (packet.SlotIndex == client.Entity.SelectedSlot)
                {
                    var clients = server.EntityManager.GetKnownClients(client.Entity);
                    foreach (var _client in clients)
                        _client.SendPacket(new EntityEquipmentPacket(client.Entity.EntityId, EntityEquipmentPacket.EntityEquipmentSlot.HeldItem,
                            client.Entity.Inventory[packet.SlotIndex]));
                }
            }
        }

        public static void HeldItemChange(RemoteClient client, MinecraftServer server, IPacket _packet)
        {
            var packet = (HeldItemChangePacket)_packet;
            if (packet.SlotIndex < 10 && packet.SlotIndex >= 0)
            {
                // TODO: Move the equipment update packet to an OnPropertyChanged event handler
                client.Entity.SelectedSlot = (short)(InventoryWindow.HotbarIndex + packet.SlotIndex);
                var clients = server.EntityManager.GetKnownClients(client.Entity);
                foreach (var _client in clients)
                    _client.SendPacket(new EntityEquipmentPacket(client.Entity.EntityId, EntityEquipmentPacket.EntityEquipmentSlot.HeldItem,
                        client.Entity.Inventory[client.Entity.SelectedSlot]));
            }
        }

        public static void ClickWindow(RemoteClient client, MinecraftServer server, IPacket _packet)
        {
            var packet = (ClickWindowPacket)_packet;
            if (packet.MouseButton == 3 && packet.Shift)
                return; // No effect in vanilla minecraft
            Window window = null;
            if (packet.WindowId == 0)
                window = client.Entity.Inventory;
            // TODO: Fetch appropriate furnace/crafting bench/etc window
            if (window == null)
                return;
            if (packet.Shift)
            {
                window.MoveToAlternateArea(packet.SlotIndex);
                return;
            }

            var heldItem = client.Entity.ItemInMouse;

            if (packet.SlotIndex == -999)
            {
                if (heldItem.Empty)
                    return;
                var entity = new ItemEntity(client.Entity.Position + new Vector3(0, client.Entity.Size.Height, 0), heldItem);
                entity.Velocity = MathHelper.FowardVector(client.Entity.Yaw) * new Vector3(0.25);
                server.EntityManager.SpawnEntity(client.Entity.World, entity);
                return;
            }

            var clickedItem = client.Entity.Inventory[packet.SlotIndex];

            if (heldItem.Empty)
            {
                if (clickedItem.Empty)
                    return;
                if (packet.MouseButton == 1)
                {
                    var heldCount = (sbyte)(clickedItem.Count / 2 + (clickedItem.Count % 2));
                    var leftCount = (sbyte)(clickedItem.Count / 2);
                    client.Entity.ItemInMouse = new ItemStack(clickedItem.Id, heldCount, clickedItem.Metadata);
                    var old = client.Entity.Inventory[packet.SlotIndex];
                    client.Entity.Inventory[packet.SlotIndex] = new ItemStack(old.Id, leftCount, old.Metadata, old.Nbt);
                }
                else
                {
                    client.Entity.ItemInMouse = clickedItem;
                    client.Entity.Inventory[packet.SlotIndex] = ItemStack.EmptyStack;
                }
            }
            else
            {
                if (packet.MouseButton == 1 && ((clickedItem.Id == heldItem.Id &&
                    clickedItem.Metadata == heldItem.Metadata) || clickedItem.Empty))
                {
                    if (!clickedItem.Empty && clickedItem.Count < Item.GetMaximumStackSize(new ItemDescriptor(clickedItem.Id)))
                    {
                        client.Entity.Inventory[packet.SlotIndex] = new ItemStack(heldItem.Id,
                            (sbyte)(clickedItem.Count + (clickedItem.Empty ? 0 : 1)), heldItem.Metadata);
                        client.Entity.ItemInMouse = new ItemStack(client.Entity.ItemInMouse.Id, (sbyte)(client.Entity.ItemInMouse.Count - 1),
                            client.Entity.ItemInMouse.Metadata, client.Entity.ItemInMouse.Nbt);
                    }
                    else
                        client.Entity.Inventory[packet.SlotIndex] = new ItemStack(heldItem.Id, 1, heldItem.Metadata);
                }
                else
                {
                    if (clickedItem.Empty)
                    {
                        client.Entity.Inventory[packet.SlotIndex] = heldItem;
                        client.Entity.ItemInMouse = ItemStack.EmptyStack;
                    }
                    else
                    {
                        client.Entity.ItemInMouse = clickedItem;
                        client.Entity.Inventory[packet.SlotIndex] = heldItem;
                    }
                }
            }
        }

        public static void CloseWindow(RemoteClient client, MinecraftServer server, IPacket _packet)
        {
            // Do nothing
            // TODO: Do something?
        }
    }
}
