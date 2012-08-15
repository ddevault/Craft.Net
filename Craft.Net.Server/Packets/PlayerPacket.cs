using System;

namespace Craft.Net.Server.Packets
{
    public class PlayerPacket : Packet
    {
        public bool OnGround;

        public override byte PacketID
        {
            get { return 0xA; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            int offset = 1;
            if (!TryReadBoolean(buffer, ref offset, out OnGround))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer server, ref MinecraftClient client)
        {
            // No action needed for this packet
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            throw new InvalidOperationException();
        }
    }
}