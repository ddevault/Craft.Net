using Craft.Net.Data;
using System;

namespace Craft.Net.Server.Packets
{
    public class PlayerPositionPacket : Packet
    {
        public bool OnGround;
        public double Stance;
        public double X, Y, Z;

        public override byte PacketId
        {
            get { return 0x0B; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            int offset = 1;
            if (!DataUtility.TryReadDouble(buffer, ref offset, out X))
                return -1;
            if (!DataUtility.TryReadDouble(buffer, ref offset, out Y))
                return -1;
            if (!DataUtility.TryReadDouble(buffer, ref offset, out Stance))
                return -1;
            if (!DataUtility.TryReadDouble(buffer, ref offset, out Z))
                return -1;
            if (!DataUtility.TryReadBoolean(buffer, ref offset, out OnGround))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer server, MinecraftClient client)
        {
            if (!client.ReadyToSpawn)
                return;
            client.Entity.FoodExhaustion += (float)client.Entity.GivenPosition.DistanceTo(new Vector3(X, Y, Z)) *
                (client.Entity.IsSprinting ? 0.1f : 0.01f); // TODO: Swimming

            if ((Y - client.Entity.GivenPosition.Y) > 0)
                client.Entity.PositiveDeltaY += (Y - client.Entity.GivenPosition.Y);
            else
                client.Entity.PositiveDeltaY = 0;

            client.Entity.GivenPosition = new Vector3(X, Y, Z);
            client.UpdateChunksAsync();
            server.ProcessSendQueue();
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            throw new InvalidOperationException();
        }
    }
}