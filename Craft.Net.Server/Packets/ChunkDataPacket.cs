using System;
using System.Linq;

namespace Craft.Net.Server.Packets
{
    public class ChunkDataPacket : Packet
    {
        public int X, Z;
        public bool GroundUpContiguous;
        public ushort PrimaryBitMap, AddBitMap;
        public byte[] CompressedData;

        public ChunkDataPacket()
        {
        }

        public override byte PacketID
        {
            get
            {
                return 0x33;
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
                .Concat(CreateBoolean(GroundUpContiguous))
                .Concat(CreateUShort(PrimaryBitMap))
                .Concat(CreateUShort(AddBitMap))
                .Concat(CreateInt(CompressedData.Length))
                .Concat(CreateInt(0))
                .Concat(CompressedData).ToArray();
            Client.SendData(buffer);
        }
    }
}

