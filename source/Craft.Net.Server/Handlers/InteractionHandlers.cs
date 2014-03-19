using Craft.Net.Common;
using Craft.Net.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Logic;
using Craft.Net.Anvil;

namespace Craft.Net.Server.Handlers
{
    internal class InteractionHandlers
    {
        public static void PlayerDigging(RemoteClient client, MinecraftServer server, IPacket _packet)
        {
            var packet = (PlayerBlockActionPacket)_packet;
            var position = new Coordinates3D(packet.X, packet.Y, packet.Z);
            // TODO: Enforce line-of-sight
            var block = client.World.GetBlockInfo(position);
            short damage;
            switch (packet.Action)
            {
                case PlayerBlockActionPacket.BlockAction.StartDigging:
                    if (client.Entity.Position.DistanceTo(position) <= client.MaxDigDistance)
                    {
                        // TODO: Block stuff
                        if (client.GameMode == GameMode.Creative || client.Entity.Abilities.InstantMine)// || Block.GetLogicDescriptor(block).Hardness == 0)
                        {
                            client.World.SetBlockId(position, 0);
                            client.World.SetMetadata(position, 0);
                        }
                        else
                        {
                            int time = 1; //Block.GetHarvestTime(new ItemDescriptor(client.Entity.SelectedItem.Id), block, out damage);
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
                case PlayerBlockActionPacket.BlockAction.CancelDigging:
                    {
                        client.BlockBreakStartTime = null;
                        var knownClients = server.EntityManager.GetKnownClients(client.Entity);
                        foreach (var c in knownClients)
                            c.SendPacket(new BlockBreakAnimationPacket(client.Entity.EntityId, position.X, position.Y, position.Z, 0xFF)); // reset
                    }
                    break;
                case PlayerBlockActionPacket.BlockAction.FinishDigging:
                    if (client.Entity.Position.DistanceTo(position) <= client.MaxDigDistance)
                    {
                        client.BlockBreakStartTime = null;
                        var knownClients = server.EntityManager.GetKnownClients(client.Entity);
                        foreach (var c in knownClients)
                            c.SendPacket(new BlockBreakAnimationPacket(client.Entity.EntityId, position.X, position.Y, position.Z, 0xFF)); // reset
                        if (client.ExpectedMiningEnd > DateTime.Now || client.ExpectedBlockToMine != position)
                            return;
                        //Block.GetHarvestTime(new ItemDescriptor(client.Entity.SelectedItem.Id), block, out damage);
                        damage = 0;
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
                        client.World.MineBlock(position); // TODO: Tools
                        client.Entity.FoodExhaustion += 0.025f;
                    }
                    break;
                case PlayerBlockActionPacket.BlockAction.DropItem:
                case PlayerBlockActionPacket.BlockAction.DropItemStack:
                    var SlotItem = client.Entity.Inventory[client.Entity.SelectedSlot];
                    if (!SlotItem.Empty)
                    {
                        var ItemCopy = (ItemStack)SlotItem.Clone();
                        if (packet.Action == PlayerBlockActionPacket.BlockAction.DropItemStack)
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
            BlockInfo? block = null;
            if (position != -Coordinates3D.One)
            {
                if (position.DistanceTo(client.Entity.Position) > client.Reach)
                    return;
                block = client.World.GetBlockInfo(position);
            }
            bool use = true;
            if (block != null)
                ;//use = Block.OnBlockRightClicked(block.Value, client.World, position, AdjustByDirection(packet.Face), cursorPosition);
            if (!slot.Empty)
            {
                //var item = new ItemDescriptor(slot.Id, slot.Metadata);
                if (use)
                {
//                    if (block != null)
//                    {
//                        Item.OnItemUsedOnBlock(item, client.World, position, AdjustByDirection(packet.Face), cursorPosition);
//                        if (client.GameMode != GameMode.Creative)
//                        {
//                            slot.Count--; // TODO: This is probably a bad place to put this code
//                            if (slot.Count == 0)
//                                client.Entity.Inventory[client.Entity.SelectedSlot] = ItemStack.EmptyStack;
//                            else
//                                client.Entity.Inventory[client.Entity.SelectedSlot] = slot;
//                        }
//                    }
//                    else
//                        Item.OnItemUsed(item);
                }
            }
        }

        private static Vector3 AdjustByDirection(BlockFace face)
        {
            switch (face)
            {
                case BlockFace.NegativeY:
                    return Vector3.Down;
                case BlockFace.PositiveY:
                    return Vector3.Up;
                case BlockFace.NegativeZ:
                    return Vector3.Backwards;
                case BlockFace.PositiveZ:
                    return Vector3.Forwards;
                case BlockFace.NegativeX:
                    return Vector3.Left;
                default:
                    return Vector3.Right;
            }
        }
    }
}
