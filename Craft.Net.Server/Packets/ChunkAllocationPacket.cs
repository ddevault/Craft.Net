using System;
using System.Linq;

namespace Craft.Net.Server.Packets
{
    public class ChunkAllocationPacket : Packet
    {
        public int X, Z;
        public bool Allocate;

        public ChunkAllocationPacket()
        {
        }

        public override byte PacketID
        {
            get
            {
                return 0x32;
            }
        }

        public override int TryReadPacket(byte[] Buffer, int Length)
        {
            throw new InvalidOperationException();
        }

        public override void HandlePacket(MinecraftServer Server, ref MinecraftClient Client)
        {
            throw new InvalidOperationException();
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {
            byte[] buffer = new byte[] { PacketID }
                .Concat(CreateInt(X))
                .Concat(CreateInt(Z))
                .Concat(CreateBoolean(Allocate)).ToArray();
            Client.SendData(buffer);
        }
    }
}

