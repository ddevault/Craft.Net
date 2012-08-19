using System.Linq;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public class PlayerPositionAndLookPacket : Packet
    {
        public bool OnGround;
        public float Pitch;
        public double Stance;
        public double X, Y;
        public float Yaw;
        public double Z;

        public PlayerPositionAndLookPacket()
        {
        }

        public PlayerPositionAndLookPacket(Vector3 position, float yaw, float pitch, bool onGround)
        {
            X = position.X;
            Y = position.Y;
            Z = position.Z;
            Stance = Y + 1.5;
            this.Yaw = yaw;
            this.Pitch = pitch;
            this.OnGround = onGround;
        }

        public override byte PacketId
        {
            get { return 0xD; }
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
            if (!DataUtility.TryReadFloat(buffer, ref offset, out Yaw))
                return -1;
            if (!DataUtility.TryReadFloat(buffer, ref offset, out Pitch))
                return -1;
            if (!DataUtility.TryReadBoolean(buffer, ref offset, out OnGround))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer server, ref MinecraftClient client)
        {
            if (!client.ReadyToSpawn)
                return;
            client.Entity.Position = new Vector3(X, Y, Z);
            client.Entity.Pitch = Pitch;
            client.Entity.Yaw = Yaw;
            if (client.Entity.Position.DistanceTo(client.Entity.OldPosition) >
                client.MaxMoveDistance)
            {
                client.SendPacket(new DisconnectPacket("Hacking: You moved too fast!"));
                server.ProcessSendQueue();
                return;
            }
            client.UpdateChunksAsync();
            server.ProcessSendQueue();
            server.EntityManager.UpdateEntity(client.Entity);
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            byte[] buffer = new[] {PacketId}
                .Concat(DataUtility.CreateDouble(X))
                .Concat(DataUtility.CreateDouble(Stance))
                .Concat(DataUtility.CreateDouble(Y))
                .Concat(DataUtility.CreateDouble(Z))
                .Concat(DataUtility.CreateFloat(Yaw))
                .Concat(DataUtility.CreateFloat(Pitch))
                .Concat(DataUtility.CreateBoolean(OnGround)).ToArray();
            client.SendData(buffer);
        }
    }
}