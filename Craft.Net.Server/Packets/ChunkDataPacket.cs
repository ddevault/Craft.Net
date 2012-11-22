using System;
using Craft.Net.Data;
using Ionic.Zlib;

namespace Craft.Net.Server.Packets
{
   public class ChunkDataPacket : Packet
   {
      public static byte[] ChunkRemovalSequence =
         new byte[]
         {
            0x78,
            0x9C,
            0x63,
            0x64,
            0x1C,
            0xD9,
            0x00,
            0x00,
            0x81,
            0x80,
            0x01,
            0x01
         };

      private const int BlockDataLength = Section.Width * Section.Depth * Section.Height;
      private const int NibbleDataLength = BlockDataLength / 2;

      public ushort AddBitMap;
      public byte[] CompressedData;
      public bool GroundUpContiguous;
      public ushort PrimaryBitMap;
      public int X, Z;

      public ChunkDataPacket()
      {
      }

      public ChunkDataPacket(ref Chunk chunk)
      {
         X = (int)chunk.AbsolutePosition.X;
         Z = (int)chunk.AbsolutePosition.Z;

         byte[] blockData;
         byte[] metadata;
         byte[] blockLight;
         byte[] skyLight;

         ushort mask = 1, chunkY = 0;
         bool nonAir = true;

         // First pass calculates number of sections to send
         int totalSections = 0;
         for (int i = 15; i >= 0; i--)
         {
            Section s = chunk.Sections [chunkY++];

            if (s.IsAir)
               nonAir = false;
            if (nonAir)
               totalSections++;
            mask <<= 1;
         }

         mask = 1;
         chunkY = 0;
         nonAir = true;
         blockData = new byte[totalSections * BlockDataLength];
         metadata = new byte[totalSections * NibbleDataLength];
         blockLight = new byte[totalSections * NibbleDataLength];
         skyLight = new byte[totalSections * NibbleDataLength];

         // Second pass produces the arrays
         for (int i = 15; i >= 0; i--)
         {
            Section s = chunk.Sections [chunkY++];

            if (s.IsAir)
               nonAir = false;
            if (nonAir)
            {
               Array.Copy(s.Blocks, 0, blockData, (chunkY - 1) * BlockDataLength, BlockDataLength);
               Array.Copy(s.Metadata.Data, 0, metadata, (chunkY - 1) * NibbleDataLength, NibbleDataLength);
               Array.Copy(s.BlockLight.Data, 0, blockLight, (chunkY - 1) * NibbleDataLength, NibbleDataLength);
               Array.Copy(s.SkyLight.Data, 0, skyLight, (chunkY - 1) * NibbleDataLength, NibbleDataLength);

               PrimaryBitMap |= mask;
            }

            mask <<= 1;
         }

         // Create the final array
         // TODO: Merge this into the other loop, reduce objects
         byte[] data = new byte[blockData.Length + metadata.Length +
            blockLight.Length + skyLight.Length + chunk.Biomes.Length];
         int index = 0;
         Array.Copy(blockData, 0, data, index, blockData.Length);
         index += blockData.Length;
         Array.Copy(metadata, 0, data, index, metadata.Length);
         index += metadata.Length;
         Array.Copy(blockLight, 0, data, index, blockLight.Length);
         index += blockLight.Length;
         Array.Copy(skyLight, 0, data, index, skyLight.Length);
         index += skyLight.Length;
         Array.Copy(chunk.Biomes, 0, data, index, chunk.Biomes.Length);

         // Compress the array
         var result = ZlibStream.CompressBuffer(data);
         CompressedData = result;

         GroundUpContiguous = true;
      }

      public override byte PacketId
      {
         get { return 0x33; }
      }

      public override int TryReadPacket(byte[] buffer, int length)
      {
         throw new InvalidOperationException();
      }

      public override void HandlePacket(MinecraftServer server, MinecraftClient client)
      {
         throw new InvalidOperationException();
      }

      public override void SendPacket(MinecraftServer server, MinecraftClient client)
      {
         client.SendData(CreateBuffer(
            DataUtility.CreateInt32(X),
            DataUtility.CreateInt32(Z),
            DataUtility.CreateBoolean(GroundUpContiguous),
            DataUtility.CreateUInt16(PrimaryBitMap),
            DataUtility.CreateUInt16(AddBitMap),
            DataUtility.CreateInt32(CompressedData.Length),
            CompressedData));
         }
      }
}