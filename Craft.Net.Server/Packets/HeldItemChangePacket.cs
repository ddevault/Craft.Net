using System;

namespace Craft.Net.Server.Packets
{
    public class HeldItemChangePacket : Packet
    {
        public short SlotId;

        public HeldItemChangePacket()
        {
        }

        public override byte PacketID
        {
            get
            {
                return 0x10;
            }
        }

        public override int TryReadPacket(byte[] Buffer, int Length)
        {
            int offset = 1;
            if (!TryReadShort(Buffer, ref offset, out SlotId))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer Server, ref MinecraftClient Client)
        {
            // TODO
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {
            throw new System.NotImplementedException();
        }
    }
}

