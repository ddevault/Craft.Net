using System;
using Craft.Net.Server.Worlds;

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
            Client.Entity.Position = new Vector3(X, Y, Z);
            if (Client.Entity.Position.DistanceTo(Client.Entity.OldPosition) > 
                MinecraftClient.MaxMoveDistance)
            {
                Client.SendPacket(new DisconnectPacket("Hacking: You moved too fast!"));
                Server.ProcessSendQueue();
                return;
            }
            Client.UpdateChunksAsync();
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {

        }
    }
}

