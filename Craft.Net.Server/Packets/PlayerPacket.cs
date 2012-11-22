using System;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public class PlayerPacket : Packet
    {
        public bool OnGround;

        public override byte PacketId
        {
            get { return 0x0A; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            int offset = 1;
            if (!DataUtility.TryReadBoolean(buffer, ref offset, out OnGround))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer server, MinecraftClient client)
        {
            // No action needed for this packet
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            throw new InvalidOperationException();
        }
    }
}