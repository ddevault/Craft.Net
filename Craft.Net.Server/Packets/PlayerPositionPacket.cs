using System;

namespace Craft.Net.Server.Packets
{
    public class PlayerPositionPacket : Packet
    {
        public double X, Y, Z, Stance;
        public bool OnGround;

        public PlayerPositionPacket()
        {
        }

        public override byte PacketID
        {
            get
            {
                return 0xB;
            }
        }

        public override int TryReadPacket(byte[] Buffer, int Length)
        {
            int offset = 1;
            if (!TryReadDouble(Buffer, ref offset, out X))
                return -1;
            if (!TryReadDouble(Buffer, ref offset, out Y))
                return -1;
            if (!TryReadDouble(Buffer, ref offset, out Stance))
                return -1;
            if (!TryReadDouble(Buffer, ref offset, out Z))
                return -1;
            if (!TryReadBoolean(Buffer, ref offset, out OnGround))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer Server, ref MinecraftClient Client)
        {
            if (!Client.ReadyToSpawn)
                return;
            // TODO
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {

        }
    }
}

