using System;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public class PlayerLookPacket : Packet
    {
        public bool OnGround;
        public float Pitch, Yaw;

        public override byte PacketId
        {
            get { return 0xC; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            int offset = 1;
            if (!DataUtility.TryReadFloat(buffer, ref offset, out Pitch))
                return -1;
            if (!DataUtility.TryReadFloat(buffer, ref offset, out Yaw))
                return -1;
            if (!DataUtility.TryReadBoolean(buffer, ref offset, out OnGround))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer server, ref MinecraftClient client)
        {
            if (!client.ReadyToSpawn)
                return;
            client.Entity.Pitch = Pitch;
            client.Entity.Yaw = Yaw;
            server.EntityManager.UpdateEntity(client.Entity);
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            throw new InvalidOperationException();
        }
    }
}