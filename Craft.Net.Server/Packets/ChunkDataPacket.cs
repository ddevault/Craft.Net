using System;
using System.Linq;
using Craft.Net.Server.Worlds;
using ICSharpCode.SharpZipLib.Zip.Compression;

namespace Craft.Net.Server.Packets
{
    public class ChunkDataPacket : Packet
    {
        public static int CompressionLevel = 6;
        static Deflater zLibDeflater;

        public int X, Z;
        public bool GroundUpContiguous;
        public ushort PrimaryBitMap, AddBitMap;
        public byte[] CompressedData;

        public ChunkDataPacket()
        {
            if (zLibDeflater == null)
                zLibDeflater = new Deflater(CompressionLevel);
        }

        public ChunkDataPacket(ref Chunk Chunk) : base()
        {
            this.X = (int)Chunk.RelativePosition.X;
            this.Z = (int)Chunk.RelativePosition.Z;

            byte[] blockData = new byte[0];
            byte[] metadata = new byte[0];
            byte[] blockLight = new byte[0];
            byte[] skyLight = new byte[0];
            
            ushort mask = 1;
            bool nonAir = true;
            for (int i = 15; i >= 0; i--)
            {
                Section s = Chunk.Sections[0];

                if (s.IsAir)
                    nonAir = false;
                if (nonAir)
                {
                    blockData = blockData.Concat(s.Blocks).ToArray();
                    metadata = metadata.Concat(s.Metadata.Data).ToArray();
                    blockLight = blockLight.Concat(s.BlockLight.Data).ToArray();
                    skyLight = skyLight.Concat(s.SkyLight.Data).ToArray();
                    
                    this.PrimaryBitMap |= mask;
                }
                
                mask <<= 1;
            }

            byte[] data = blockData.Concat(metadata).Concat(blockLight)
                .Concat(skyLight).Concat(Chunk.Biomes).ToArray();
            zLibDeflater.SetInput(data);
            zLibDeflater.Finish();
            int length = zLibDeflater.Deflate(data);
            zLibDeflater.Reset();

            CompressedData = new byte[length];
            Array.Copy(data, CompressedData, length);
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

