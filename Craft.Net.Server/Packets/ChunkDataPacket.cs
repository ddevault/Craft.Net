using System;
using System.Linq;
using Craft.Net.Server.Worlds;
using ICSharpCode.SharpZipLib.Zip.Compression;

namespace Craft.Net.Server.Packets
{
    public class ChunkDataPacket : Packet
    {
        public static int CompressionLevel = 5;
        static Deflater zLibDeflater;
        public static byte[] ChunkRemovalSequence =
            new byte[] { 0x78, 0x9C, 0x63, 0x64, 0x1C, 0xD9, 0x00, 0x00, 0x81, 0x80, 0x01, 0x01 };

        public int X, Z;
        public bool GroundUpContiguous;
        public ushort PrimaryBitMap, AddBitMap;
        public byte[] CompressedData;

        public ChunkDataPacket()
        {
            if (zLibDeflater == null)
                zLibDeflater = new Deflater(CompressionLevel);
        }

        public ChunkDataPacket(ref Chunk Chunk) : this()
        {
            this.X = (int)Chunk.AbsolutePosition.X;
            this.Z = (int)Chunk.AbsolutePosition.Z;

            byte[] blockData = new byte[0];
            byte[] metadata = new byte[0];
            byte[] blockLight = new byte[0];
            byte[] skyLight = new byte[0];
            
            ushort mask = 1, chunkY = 0;
            bool nonAir = true;
            for (int i = 15; i >= 0; i--)
            {
                Section s = Chunk.Sections[chunkY++];

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
            int uncompressed = data.Length;
            int length = zLibDeflater.Deflate(data);
            Console.WriteLine("Before compression: " + uncompressed + ", after: " + length);
            zLibDeflater.Reset();

            this.GroundUpContiguous = true;

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

