using System;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public class CreativeInventoryActionPacket : Packet
    {
        public short Index;
        public Slot Item;

        public override byte PacketId
        {
            get { return 0x6B; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            int offset = 1;
            if (!DataUtility.TryReadInt16(buffer, ref offset, out Index))
                return -1;
            if (!Slot.TryReadSlot(buffer, ref offset, out Item))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer server, ref MinecraftClient client)
        {
            if (Index < client.Inventory.Length)
                client.Inventory[Index] = Item;
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            throw new InvalidOperationException();
        }
    }
}