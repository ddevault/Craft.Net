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
                        if (client.GameMode == GameMode.Creative || client.Entity.Abilities.InstantMine)// || Block.GetBlockHardness(block.BlockId) == 0)
                        {
                            client.World.SetBlockId(position, 0);
                            client.World.SetMetadata(position, 0);
                        }
                        else
                        {
                            int time = Block.GetHarvestTime(block.BlockId, client.Entity.SelectedItem.Id, client.World, client.Entity, out damage);
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
                        Block.GetHarvestTime(block.BlockId, client.Entity.SelectedItem.Id, client.World, client.Entity, out damage);
                        if (damage != 0)
                        {
                            var slot = client.Entity.Inventory[client.Entity.SelectedSlot];
                            if (!slot.Empty)
                            {
                                if (slot.AsItem() != null)
                                {
                                    var item = slot.AsItem().Value;
                                    if (Item.GetToolType(item.ItemId) != null)
                                    {
                                        bool destroyed = Item.Damage(ref item, damage);
                                        slot.Metadata = item.Metadata;
                                        if (destroyed)
                                            client.Entity.Inventory[client.Entity.SelectedSlot] = ItemStack.EmptyStack;
                                        else
                                            client.Entity.Inventory[client.Entity.SelectedSlot] = slot;
                                    }
                                }
                            }
                        }
                        client.World.MineBlock(position);
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
                if (position.DistanceTo((Coordinates3D)client.Entity.Position) > client.Reach)
                    return;
                block = client.World.GetBlockInfo(position);
            }
            bool use = true;
            if (block != null)
                use = client.World.RightClickBlock(position, packet.Face, cursorPosition, slot.AsItem());
            if (!slot.Empty)
            {
                var item = slot.AsItem();
                if (use)
                {
                    if (block != null)
                    {
                        client.World.UseItemOnBlock(position, packet.Face, cursorPosition, item.Value);
                        if (item.Value.ItemId < 0x100)
                        {
                            client.SendPacket(new SoundEffectPacket(Block.GetPlacementSoundEffect(item.Value.ItemId),
                                position.X, position.Y, position.Z, SoundEffectPacket.DefaultVolume, SoundEffectPacket.DefaultPitch));
                        }
                        if (client.GameMode != GameMode.Creative)
                        {
                            slot.Count--; // TODO: This is probably a bad place to put this code
                            if (slot.Count == 0)
                                client.Entity.Inventory[client.Entity.SelectedSlot] = ItemStack.EmptyStack;
                            else
                                client.Entity.Inventory[client.Entity.SelectedSlot] = slot;
                        }
                    }
                    else
                    {
                        client.World.UseItemOnBlock(position, packet.Face, cursorPosition, item.Value);
                        if (item.Value.ItemId < 0x100)
                        {
                            client.SendPacket(new SoundEffectPacket(Block.GetPlacementSoundEffect(item.Value.ItemId),
                                position.X, position.Y, position.Z, SoundEffectPacket.DefaultVolume, SoundEffectPacket.DefaultPitch));
                        }
                    }
                }
            }
        }
    }
}
