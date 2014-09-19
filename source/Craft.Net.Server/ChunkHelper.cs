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
		public static byte[] short2byteArr (short[] arr)
		{
			System.IO.MemoryStream ms = new System.IO.MemoryStream (); 
			System.IO.BinaryWriter bw = new System.IO.BinaryWriter (ms);
			for (int i = 0; i < arr.Length-1;) { 

				Int32 tmp = (int)(arr [i++] | (arr [i++] << 12)); 
				bw.Write (tmp);
			}
			return (byte[])ms.ToArray (); 
		}

		public static byte[] ChunkRemovalSequence = new byte[] {
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
		private const int BlockDataLength = Section.Width * Section.Height * Section.Depth;
		private const int NibbleDataLength = BlockDataLength / 2;

		public static ChunkDataPacket CreatePacket (Chunk chunk)
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
			for (int i = 15; i >= 0; i--) {
				Section s = chunk.Sections [chunkY++];

				if (s.IsAir)
					nonAir = false;
				if (nonAir)
					totalSections++;
			}

			mask = 1;
			chunkY = 0;
			nonAir = true;
			//Yes I hard-coded these, I'll un-hard-code them later
			blockData = new short[(int)totalSections * (int)(Math.Pow (16, 3) * (5 + 1) / 2) + 256];
			//metadata = new byte[totalSections * NibbleDataLength];
			blockLight = new byte[totalSections * NibbleDataLength];
			skyLight = new byte[totalSections * NibbleDataLength];

			ushort PrimaryBitMap = 0;

			// Second pass produces the arrays
			for (int i = 15; i >= 0; i--) {
				Section s = chunk.Sections [chunkY++];
				short[] tempblocks = s.Blocks;

				for (int x = 0; x < s.Blocks.Length; x++) {
					int blockid = (s.Blocks [x] << 4) | (s.Blocks [x] & 0xf);
					tempblocks [x] = (short)blockid;
				}

				if (s.IsAir)
					nonAir = false;
				if (nonAir) {
					Array.Copy (short2byteArr (tempblocks), 0, blockData, (chunkY - 1) * BlockDataLength, BlockDataLength);
					//		Array.Copy(s.Metadata.Data, 0, metadata, (chunkY - 1) * NibbleDataLength, NibbleDataLength);
					Array.Copy (s.BlockLight.Data, 0, blockLight, (chunkY - 1) * NibbleDataLength, NibbleDataLength);
					Array.Copy (s.SkyLight.Data, 0, skyLight, (chunkY - 1) * NibbleDataLength, NibbleDataLength);

					PrimaryBitMap |= mask;
				}

				mask <<= 1;
			}
			// Create the final array
			// TODO: Merge this into the other loop, reduce objects
			byte[] data = new byte[blockData.Length +
				blockLight.Length + skyLight.Length + chunk.Biomes.Length];
			int index = 0;
			Array.Copy (short2byteArr (blockData), 0, data, index, blockData.Length);
			index += blockData.Length;
			//Array.Copy(metadata, 0, data, index, metadata.Length);
			//index += metadata.Length;
			Array.Copy (blockLight, 0, data, index, blockLight.Length);
			index += blockLight.Length;
			Array.Copy (skyLight, 0, data, index, skyLight.Length);
			index += skyLight.Length;
			Array.Copy (chunk.Biomes, 0, data, index, chunk.Biomes.Length);

			var GroundUpContiguous = true;
			return new ChunkDataPacket (X, Z, GroundUpContiguous, PrimaryBitMap, data);
		}
	}
}