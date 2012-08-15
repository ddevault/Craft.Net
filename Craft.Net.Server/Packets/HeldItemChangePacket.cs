using System;

namespace Craft.Net.Server.Packets
{
    public class HeldItemChangePacket : Packet
    {
        public short SlotId;

        public override byte PacketID
        {
            get { return 0x10; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            int offset = 1;
            if (!TryReadShort(buffer, ref offset, out SlotId))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer server, ref MinecraftClient client)
        {
            // TODO
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            throw new NotImplementedException();
        }
    }
}