using System;
using System.Linq;
using Craft.Net.Data;
using ICSharpCode.SharpZipLib.Zip.Compression;

namespace Craft.Net.Server.Packets
{
    public class ChunkDataPacket : Packet
    {
        public static int CompressionLevel = 5;
        private static Deflater zLibDeflater;

        public static byte[] ChunkRemovalSequence =
            new byte[] {0x78, 0x9C, 0x63, 0x64, 0x1C, 0xD9, 0x00, 0x00, 0x81, 0x80, 0x01, 0x01};

        private static object lockObject;

        public ushort AddBitMap;
        public byte[] CompressedData;
        public bool GroundUpContiguous;
        public ushort PrimaryBitMap;
        public int X, Z;

        public ChunkDataPacket()
        {
            if (zLibDeflater == null)
                zLibDeflater = new Deflater(CompressionLevel);
            if (lockObject == null)
                lockObject = new object();
        }

        public ChunkDataPacket(ref Chunk chunk) : this()
        {
            X = (int)chunk.AbsolutePosition.X;
            Z = (int)chunk.AbsolutePosition.Z;

            var blockData = new byte[0];
            var metadata = new byte[0];
            var blockLight = new byte[0];
            var skyLight = new byte[0];

            ushort mask = 1, chunkY = 0;
            bool nonAir = true;
            for (int i = 15; i >= 0; i--)
            {
                Section s = chunk.Sections[chunkY++];

                if (s.IsAir)
                    nonAir = false;
                if (nonAir)
                {
                    blockData = blockData.Concat(s.Blocks).ToArray();
                    metadata = metadata.Concat(s.Metadata.Data).ToArray();
                    blockLight = blockLight.Concat(s.BlockLight.Data).ToArray();
                    skyLight = skyLight.Concat(s.SkyLight.Data).ToArray();

                    PrimaryBitMap |= mask;
                }

                mask <<= 1;
            }

            byte[] data = blockData.Concat(metadata).Concat(blockLight)
                .Concat(skyLight).Concat(chunk.Biomes).ToArray();
            int length;
            var result = new byte[data.Length];
            lock (lockObject)
            {
                zLibDeflater.SetInput(data);
                zLibDeflater.Finish();
                length = zLibDeflater.Deflate(result);
                zLibDeflater.Reset();
            }

            GroundUpContiguous = true;

            CompressedData = result.Take(length).ToArray();
        }

        public override byte PacketId
        {
            get { return 0x33; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            throw new InvalidOperationException();
        }

        public override void HandlePacket(MinecraftServer server, ref MinecraftClient client)
        {
            throw new InvalidOperationException();
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            byte[] buffer = new[] {PacketId}
                .Concat(DataUtility.CreateInt(X))
                .Concat(DataUtility.CreateInt(Z))
                .Concat(DataUtility.CreateBoolean(GroundUpContiguous))
                .Concat(DataUtility.CreateUShort(PrimaryBitMap))
                .Concat(DataUtility.CreateUShort(AddBitMap))
                .Concat(DataUtility.CreateInt(CompressedData.Length))
                .Concat(CompressedData).ToArray();
            client.SendData(buffer);
        }
    }
}