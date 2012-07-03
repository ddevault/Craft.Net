using System;
using Craft.Net.Server.Worlds;
using System.Linq;

namespace Craft.Net.Server.Packets
{
    public class PlayerPositionAndLookPacket : Packet
    {
        public double X, Y, Z, Stance;
        public float Yaw, Pitch;
        public bool OnGround;

        public PlayerPositionAndLookPacket()
        {
        }

        public PlayerPositionAndLookPacket(Vector3 Position, float Yaw, float Pitch, bool OnGround)
        {
        }

        public override byte PacketID
        {
            get
            {
                return 0xD;
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
            // TODO
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {
            byte[] buffer = new byte[] { PacketID }
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

