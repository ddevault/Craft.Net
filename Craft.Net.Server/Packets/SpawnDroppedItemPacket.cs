using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data;
using Craft.Net.Data.Entities;

namespace Craft.Net.Server.Packets
{
    public class SpawnDroppedItemPacket : Packet
    {
        public ItemEntity Item;

        public SpawnDroppedItemPacket()
        {
        }

        public SpawnDroppedItemPacket(ItemEntity item)
        {
            Item = item;
        }

        public override byte PacketId
        {
            get { return 0x15; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            throw new InvalidOperationException();
        }

        public override void HandlePacket(MinecraftServer server, MinecraftClient client)
        {
            throw new InvalidOperationException();
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            // TODO: Refactor like mad
            var buffer = new List<byte>();
            buffer.Add(PacketId);
            buffer.AddRange(DataUtility.CreateInt32(Item.Id));
            if (Item.Item.Empty)
                buffer.AddRange(DataUtility.CreateInt16(-1));
            else
            {
                buffer.AddRange(DataUtility.CreateInt16((short)Item.Item.Id)); // TODO: Make slots use signed shorts
                buffer.Add(Item.Item.Count);
                buffer.AddRange(DataUtility.CreateInt16((short)Item.Item.Metadata));
                buffer.AddRange(DataUtility.CreateInt16(-1)); // TODO: Nbt appears to be sent here
            }

            buffer.AddRange(DataUtility.CreateAbsoluteInteger(Item.Position.X));
            buffer.AddRange(DataUtility.CreateAbsoluteInteger(Item.Position.Y));
            buffer.AddRange(DataUtility.CreateAbsoluteInteger(Item.Position.Z));
            buffer.AddRange(DataUtility.CreatePackedByte(Item.Pitch));
            buffer.AddRange(DataUtility.CreatePackedByte(Item.Pitch));
            buffer.AddRange(DataUtility.CreatePackedByte(Item.Yaw));

            client.SendData(buffer.ToArray());
        }
    }
}