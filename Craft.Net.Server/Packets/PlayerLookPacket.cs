using System;

namespace Craft.Net.Server.Packets
{
    public class PlayerLookPacket : Packet
    {
        public bool OnGround;
        public float Pitch, Yaw;

        public override byte PacketID
        {
            get { return 0xC; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            int offset = 1;
            if (!TryReadFloat(buffer, ref offset, out Pitch))
                return -1;
            if (!TryReadFloat(buffer, ref offset, out Yaw))
                return -1;
            if (!TryReadBoolean(buffer, ref offset, out OnGround))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer server, ref MinecraftClient client)
        {
            if (!client.ReadyToSpawn)
                return;
            client.Entity.Pitch = Pitch;
            client.Entity.Yaw = Yaw;
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            throw new InvalidOperationException();
        }
    }
}