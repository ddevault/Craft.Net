using System;
using Craft.Net.Server.Blocks;
using Craft.Net.Server.Items;

namespace Craft.Net.Server.Packets
{
    public class CreativeInventoryActionPacket : Packet
    {
        public short Index;
        public Slot Item;

        public CreativeInventoryActionPacket()
        {
        }

        public override byte PacketID
        {
            get
            {
                return 0x6B;
            }
        }

        public override int TryReadPacket(byte[] Buffer, int Length)
        {
            int offset = 1;
            if (!TryReadShort(Buffer, ref offset, out Index))
                return -1;
            if (!Slot.TryReadSlot(Buffer, ref offset, out Item))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer Server, ref MinecraftClient Client)
        {
            if (Index < Client.Inventory.Length)
                Client.Inventory[Index] = Item;
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {
            throw new InvalidOperationException();
        }
    }
}

