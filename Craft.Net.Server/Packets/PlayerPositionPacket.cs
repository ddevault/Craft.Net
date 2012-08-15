using Craft.Net.Server.Worlds;

namespace Craft.Net.Server.Packets
{
    public class PlayerPositionPacket : Packet
    {
        public bool OnGround;
        public double Stance;
        public double X, Y, Z;

        public override byte PacketID
        {
            get { return 0xB; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            int offset = 1;
            if (!TryReadDouble(buffer, ref offset, out X))
                return -1;
            if (!TryReadDouble(buffer, ref offset, out Y))
                return -1;
            if (!TryReadDouble(buffer, ref offset, out Stance))
                return -1;
            if (!TryReadDouble(buffer, ref offset, out Z))
                return -1;
            if (!TryReadBoolean(buffer, ref offset, out OnGround))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer server, ref MinecraftClient client)
        {
            if (!client.ReadyToSpawn)
                return;
            client.Entity.Position = new Vector3(X, Y, Z);
            if (client.Entity.Position.DistanceTo(client.Entity.OldPosition) >
                client.MaxMoveDistance)
            {
                client.SendPacket(new DisconnectPacket("Hacking: You moved too fast!"));
                server.ProcessSendQueue();
                return;
            }
            client.UpdateChunksAsync();
            server.GetClientWorld(client).EntityManager.UpdateEntity(client.Entity);
            server.ProcessSendQueue();
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
        }
    }
}