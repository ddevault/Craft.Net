using System;

namespace Craft.Net.Server.Packets
{
    public class PlayerLookPacket : Packet
    {
        public float Pitch, Yaw;
        public bool OnGround;

        public PlayerLookPacket()
        {
        }

        public override byte PacketID
        {
            get
            {
                return 0xC;
            }
        }

        public override int TryReadPacket(byte[] Buffer, int Length)
        {
            int offset = 1;
            if (!TryReadFloat(Buffer, ref offset, out Pitch))
                return -1;
            if (!TryReadFloat(Buffer, ref offset, out Yaw))
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
            throw new InvalidOperationException();
        }
    }
}

