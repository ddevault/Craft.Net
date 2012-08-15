using System.Linq;
using Craft.Net.Server.Worlds;

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

        public PlayerPositionAndLookPacket(Vector3 Position, float Yaw, float Pitch, bool OnGround)
        {
            X = Position.X;
            Y = Position.Y;
            Z = Position.Z;
            Stance = Y + 1.5;
            this.Yaw = Yaw;
            this.Pitch = Pitch;
            this.OnGround = OnGround;
        }

        public override byte PacketID
        {
            get { return 0xD; }
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
            if (!TryReadFloat(Buffer, ref offset, out Yaw))
                return -1;
            if (!TryReadFloat(Buffer, ref offset, out Pitch))
                return -1;
            if (!TryReadBoolean(Buffer, ref offset, out OnGround))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer Server, ref MinecraftClient Client)
        {
            if (!Client.ReadyToSpawn)
                return;
            Client.Entity.Position = new Vector3(X, Y, Z);
            Client.Entity.Pitch = Pitch;
            Client.Entity.Yaw = Yaw;
            if (Client.Entity.Position.DistanceTo(Client.Entity.OldPosition) >
                Client.MaxMoveDistance)
            {
                Client.SendPacket(new DisconnectPacket("Hacking: You moved too fast!"));
                Server.ProcessSendQueue();
                return;
            }
            Client.UpdateChunksAsync();
            Server.GetClientWorld(Client).EntityManager.UpdateEntity(Client.Entity);
            Server.ProcessSendQueue();
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {
            byte[] buffer = new[] {PacketID}
                .Concat(CreateDouble(X))
                .Concat(CreateDouble(Stance))
                .Concat(CreateDouble(Y))
                .Concat(CreateDouble(Z))
                .Concat(CreateFloat(Yaw))
                .Concat(CreateFloat(Pitch))
                .Concat(CreateBoolean(OnGround)).ToArray();
            Client.SendData(buffer);
        }
    }
}