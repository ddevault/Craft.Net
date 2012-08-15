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

        public override int TryReadPacket(byte[] Buffer, int Length)
        {
            int offset = 1;
            if (!TryReadBoolean(Buffer, ref offset, out OnGround))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer Server, ref MinecraftClient Client)
        {
            // No action needed for this packet
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {
            throw new InvalidOperationException();
        }
    }
}