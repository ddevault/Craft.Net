using Craft.Net.Anvil;
using Craft.Net.Networking;
using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server
{
    public static class ChunkHelper
    {
        //This may be correct or incorrect, I have not tested this
        public static byte[] short2byteArr(short[] arr)
        {
            byte[] Converted = Array.ConvertAll<short, byte>(arr, delegate(short
                                                             item){return (byte)item;});
            return Converted; 
        }

        public static byte[] ChunkRemovalSequence = new byte[]
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
        private const int BlockDataLength = Section.Width * Section.Height * Section.Depth * 2;
        private const int NibbleDataLength = BlockDataLength / 4;

        public static ChunkDataPacket CreatePacket(Chunk chunk)
        {
            var X = chunk.X;
            var Z = chunk.Z;
            short[] blockData;
            //byte[] metadata;
            byte[] blockLight;
            byte[] skyLight;

            ushort mask = 1, chunkY = 0;
            bool nonAir = true;

            // First pass calculates number of sections to send
            int totalSections = 0;
            for (int i = 15; i >= 0; i--)
            {
                Section s = chunk.Sections[chunkY++];

                if (s.IsAir)
                    nonAir = false;
                if (nonAir)
                    totalSections++;
            }

            mask = 1;
            chunkY = 0;
            nonAir = true;
            //Yes I hard-coded these, I'll un-hard-code them later
            blockData = new short[(int)totalSections * 2 * 16 * 16 * 16];
            //metadata = new byte[totalSections * NibbleDataLength];
            blockLight = new byte[totalSections * NibbleDataLength];
            skyLight = new byte[totalSections * NibbleDataLength];

            ushort PrimaryBitMap = 0;

            // Second pass produces the arrays
            for (int i = 15; i >= 0; i--)
            {
                Section s = chunk.Sections[chunkY++];

                byte[] tempBlock = short2byteArr(s.Blocks);
                for (int x = 0; x < s.Blocks.Length; x++)
                {
                    int blockid = (tempBlock[x] << 4) | (tempBlock[x] & 0xf);
                    tempBlock[x] = (byte)blockid;
                }

                if (s.IsAir)
                    nonAir = false;
                if (nonAir)
                {
                    //Array.Copy(tempBlock, 0, blockData, (chunkY - 1) * BlockDataLength, BlockDataLength);
                    //		Array.Copy(s.Metadata.Data, 0, metadata, (chunkY - 1) * NibbleDataLength, NibbleDataLength);
                    Array.Copy(s.BlockLight.Data, 0, blockLight, (chunkY - 1) * NibbleDataLength, NibbleDataLength);
                    Array.Copy(s.SkyLight.Data, 0, skyLight, (chunkY - 1) * NibbleDataLength, NibbleDataLength);

                    PrimaryBitMap |= mask;
                }

                mask <<= 1;
            }
            // Create the final array
            // TODO: Merge this into the other loop, reduce objects
            byte[] data = new byte[blockData.Length +
                blockLight.Length + skyLight.Length + chunk.Biomes.Length];
            int index = 0;
            Array.Copy(short2byteArr(blockData), 0, data, index, blockData.Length);
            index += blockData.Length;
            //Array.Copy(metadata, 0, data, index, metadata.Length);
            //index += metadata.Length;
            Array.Copy(blockLight, 0, data, index, blockLight.Length);
            index += blockLight.Length;
            Array.Copy(skyLight, 0, data, index, skyLight.Length);
            index += skyLight.Length;
            Array.Copy(chunk.Biomes, 0, data, index, chunk.Biomes.Length);

            var GroundUpContiguous = true;
            return new ChunkDataPacket(X, Z, GroundUpContiguous, PrimaryBitMap, data);
        }
    }
}