using System;
using Craft.Net.Data;
using Craft.Net.Data.Blocks;
using Craft.Net.Data.Items;

namespace Craft.Net.Server.Packets
{
    public enum PlayerAction
    {
        StartedDigging = 0,
        FinishedDigging = 2,
        DropItem = 4,
        ShootArrow = 5
    }

    public class PlayerDiggingPacket : Packet
    {
        public static double MaxDigDistance = 6;

        public PlayerAction Action;
        public byte Face;
        public Vector3 Position;

        public override byte PacketId
        {
            get { return 0x0E; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            int offset = 1;
            byte action, y;
            int x, z;
            if (!DataUtility.TryReadByte(buffer, ref offset, out action))
                return -1;
            if (!DataUtility.TryReadInt32(buffer, ref offset, out x))
                return -1;
            if (!DataUtility.TryReadByte(buffer, ref offset, out y))
                return -1;
            if (!DataUtility.TryReadInt32(buffer, ref offset, out z))
                return -1;
            if (!DataUtility.TryReadByte(buffer, ref offset, out Face))
                return -1;
            Position = new Vector3(x, y, z);
            Action = (PlayerAction)action;
            return offset;
        }

        public override void HandlePacket(MinecraftServer server, MinecraftClient client)
        {
            if (client.Entity.Position.DistanceTo(Position) <= MaxDigDistance)
            {
                // TODO: Enforce line-of-sight
                var block = client.World.GetBlock(Position);
                int damage;
                switch (Action)
                {
                    case PlayerAction.StartedDigging:
                        if (client.Entity.Abilities.InstantMine || block.Hardness == 0)
                            block.OnBlockMined(client.World, Position, client.Entity);
                        else
                        {
                            // TODO: Investigate exploitability with respect to ping time
                            client.ExpectedMiningEnd = DateTime.Now.AddMilliseconds(
                                block.GetHarvestTime(client.Entity.SelectedItem.Item,
                                client.World, client.Entity, out damage) - (client.Ping + 100));
                            client.ExpectedBlockToMine = Position;
                        }
                        break;
                    case PlayerAction.FinishedDigging:
                        // TODO: Check that they're finishing the same block as before
                        if (client.ExpectedMiningEnd > DateTime.Now || client.ExpectedBlockToMine != Position)
                            return;
                        block.GetHarvestTime(client.Entity.SelectedItem.Item,
                                client.World, client.Entity, out damage);
                        if (damage != 0)
                        {
                            var slot = client.Entity.Inventory[client.Entity.SelectedSlot];
                            if (!slot.Empty)
                            {
                                if (slot.Item is ToolItem)
                                {
                                    var tool = slot.Item as ToolItem;
                                    bool destroy = tool.Damage(damage);
                                    slot.Metadata = tool.Data;
                                    if (destroy)
                                        client.Entity.SetSlot(client.Entity.SelectedSlot, new Slot());
                                    else
                                        client.Entity.SetSlot(client.Entity.SelectedSlot, slot);
                                }
                            }
                        }
                        //client.World.SetBlock(Position, new AirBlock());
                        block.OnBlockMined(client.World, Position, client.Entity);
                        client.Entity.FoodExhaustion += 0.025f;
                        break;
                }
            }
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            throw new InvalidOperationException();
        }
    }
}