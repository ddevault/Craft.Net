using Craft.Net.Common;
using Craft.Net.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Logic;
using Craft.Net.Logic.Blocks;
using Craft.Net.Anvil;
using Craft.Net.Logic.Items;
using Craft.Net.Entities;

namespace Craft.Net.Server.Handlers
{
    internal class InteractionHandlers
    {
        public static double MaxDigDistance = 6; // TODO: Move somewhere else

        public static void PlayerDigging(RemoteClient client, MinecraftServer server, IPacket _packet)
        {
            var packet = (PlayerDiggingPacket)_packet;
            var position = new Coordinates3D(packet.X, packet.Y, packet.Z);
            // TODO: Enforce line-of-sight
            var block = client.World.GetBlock(position);
            short damage;
            switch (packet.Action)
            {
                case PlayerDiggingPacket.PlayerAction.StartedDigging:
                    if (client.Entity.Position.DistanceTo(position) <= MaxDigDistance)
                    {
                        if (client.GameMode == GameMode.Creative || client.Entity.Abilities.InstantMine || Block.GetLogicDescriptor(block).Hardness == 0)
                            Block.OnBlockMined(block, client.World, position, null); // TODO: Tool
                        else
                        {
                            int time = Block.GetHarvestTime(new ItemDescriptor(client.Entity.SelectedItem.Id), block, out damage);
                            client.ExpectedMiningEnd = DateTime.Now.AddMilliseconds(time - (client.Ping + 100));
                            client.ExpectedBlockToMine = position;
                            var knownClients = server.EntityManager.GetKnownClients(client.Entity);
                            client.BlockBreakStageTime = time / 8;
                            client.BlockBreakStartTime = DateTime.Now;
                            foreach (var c in knownClients)
                                c.SendPacket(new BlockBreakAnimationPacket(client.Entity.EntityId, position.X, position.Y, position.Z, 0));
                        }
                    }
                    break;
                case PlayerDiggingPacket.PlayerAction.CancelDigging:
                    {
                        client.BlockBreakStartTime = null;
                        var knownClients = server.EntityManager.GetKnownClients(client.Entity);
                        foreach (var c in knownClients)
                            c.SendPacket(new BlockBreakAnimationPacket(client.Entity.EntityId, position.X, position.Y, position.Z, 0xFF)); // reset
                    }
                    break;
                case PlayerDiggingPacket.PlayerAction.FinishedDigging:
                    if (client.Entity.Position.DistanceTo(position) <= MaxDigDistance)
                    {
                        client.BlockBreakStartTime = null;
                        var knownClients = server.EntityManager.GetKnownClients(client.Entity);
                        foreach (var c in knownClients)
                            c.SendPacket(new BlockBreakAnimationPacket(client.Entity.EntityId, position.X, position.Y, position.Z, 0xFF)); // reset
                        if (client.ExpectedMiningEnd > DateTime.Now || client.ExpectedBlockToMine != position)
                            return;
                        Block.GetHarvestTime(new ItemDescriptor(client.Entity.SelectedItem.Id), block, out damage);
                        if (damage != 0)
                        {
                            var slot = client.Entity.Inventory[client.Entity.SelectedSlot];
                            if (!slot.Empty)
                            {
                                //if (slot.AsItem() is ToolItem)
                                //{
                                //    var tool = slot.AsItem() as ToolItem;
                                //    bool destroy = tool.Damage(damage);
                                //    slot.Metadata = tool.Data;
                                //    if (destroy)
                                //        client.Entity.SetSlot(client.Entity.SelectedSlot, ItemStack.EmptyStack);
                                //    else
                                //        client.Entity.SetSlot(client.Entity.SelectedSlot, slot);
                                //}
                            }
                        }
                        Block.OnBlockMined(block, client.World, position, null);
                        client.Entity.FoodExhaustion += 0.025f;
                    }
                    break;
                case PlayerDiggingPacket.PlayerAction.DropItem:
                case PlayerDiggingPacket.PlayerAction.DropStack:
                    var SlotItem = client.Entity.Inventory[client.Entity.SelectedSlot];
                    if (!SlotItem.Empty)
                    {
                        var ItemCopy = (ItemStack)SlotItem.Clone();
                        if (packet.Action == PlayerDiggingPacket.PlayerAction.DropStack)
                            client.Entity.Inventory[client.Entity.SelectedSlot] = ItemStack.EmptyStack;
                        else
                        {
                            ItemCopy.Count = 1;
                            SlotItem.Count--; // Decrease the player's item by 1
                            if (SlotItem.Count == 0)
                                client.Entity.Inventory[client.Entity.SelectedSlot] = ItemStack.EmptyStack;
                            else
                                client.Entity.Inventory[client.Entity.SelectedSlot] = SlotItem;
                        }
                        var entity = new ItemEntity(client.Entity.Position +
                            new Vector3(0, client.Entity.Size.Height, 0), ItemCopy);
                        entity.Velocity = MathHelper.FowardVector(client.Entity.Yaw) * new Vector3(0.25);
                        server.EntityManager.SpawnEntity(client.World, entity);
                    }
                    break;
            }
        }

        public static void RightClick(RemoteClient client, MinecraftServer server, IPacket _packet)
        {
            var packet = (RightClickPacket)_packet;
            var slot = client.Entity.Inventory[client.Entity.SelectedSlot];
            var position = new Coordinates3D(packet.X, packet.Y, packet.Z);
            var cursorPosition = new Coordinates3D(packet.CursorX, packet.CursorY, packet.CursorZ);
            BlockDescriptor? block = null;
            if (position != -Coordinates3D.One)
            {
                if (position.DistanceTo(client.Entity.Position) > client.Reach)
                    return;
                block = client.World.GetBlock(position);
            }
            bool use = true;
            if (block != null)
                use = Block.OnBlockRightClicked(block.Value, client.World, position, AdjustByDirection(packet.Direction), cursorPosition);
            if (!slot.Empty)
            {
                var item = new ItemDescriptor(slot.Id);
                if (use)
                {
                    if (block != null)
                        Item.OnItemUsedOnBlock(item, client.World, position, AdjustByDirection(packet.Direction), cursorPosition);
                    else
                        Item.OnItemUsed(item);
                }
            }
        }

        private static Vector3 AdjustByDirection(byte direction)
        {
            switch (direction)
            {
                case 0:
                    return Vector3.Down;
                case 1:
                    return Vector3.Up;
                case 2:
                    return Vector3.Backwards;
                case 3:
                    return Vector3.Forwards;
                case 4:
                    return Vector3.Left;
                default:
                    return Vector3.Right;
            }
        }
    }
}
