using System;
using Craft.Net.Server.Blocks;
using ICSharpCode.SharpZipLib.Zip.Compression;

namespace Craft.Net.Server.Worlds
{
    public class Section
    {
        public const byte Width = 16, Height = 16, Depth = 16;
        public static Deflater Deflater;

        public byte Y;
        public byte[] Blocks;
        public NibbleArray Metadata;
        public NibbleArray BlockLight;
        public NibbleArray SkyLight;
        private int NonairCount;
        public bool IsAir
        {
            get
            {
                return NonairCount == 0;
            }
        }

        public Section(byte Y)
        {
            this.Y = Y;
            this.Blocks = new byte[Width * Height * Depth];
            this.Metadata = new NibbleArray(Width * Height * Depth);
            this.BlockLight = new NibbleArray(Width * Height * Depth);
            this.SkyLight = new NibbleArray(Width * Height * Depth);
            for (int i = 0; i < this.SkyLight.Data.Length; i++)
                this.SkyLight.Data[i] = 0xFF;
            this.NonairCount = 0;
        }

        /// <summary>
        /// Sets the value of the block at the given position, relative to this section.
        /// </summary>
        public void SetBlock(Vector3 position, Block value)
        {
            int x = (int)position.X;
            int y = (int)position.Y;
            int z = (int)position.Z;
            int index = x + (z * Width) + (y * Height * Width);
            this.Blocks[index] = value.BlockID;
            this.Metadata[index] = value.Metadata;
            this.BlockLight[index] = value.BlockLight;
            this.SkyLight[index] = value.SkyLight;
            if (value is AirBlock)
                NonairCount--;
            else
                NonairCount++;
        }

        public Block GetBlock(Vector3 position)
        {
            int x = (int)position.X;
            int y = (int)position.Y;
            int z = (int)position.Z;
            int index = x + (z * Width) + (y * Height * Width);
            Block block = (Block)this.Blocks[index];
            block.Metadata = this.Metadata[index];
            block.SkyLight = this.SkyLight[index];
            block.BlockLight = this.BlockLight[index];
            return block;
        }
    }
}

