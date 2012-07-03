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
        public bool IsAir;

        public Section(byte Y)
        {
            this.IsAir = true;
            this.Y = Y;
            this.Blocks = new byte[Width * Height * Depth];
            this.Metadata = new NibbleArray(Width * Height * Depth);
            this.BlockLight = new NibbleArray(Width * Height * Depth);
            this.SkyLight = new NibbleArray(Width * Height * Depth);
            for (int i = 0; i < this.SkyLight.Data.Length; i++)
                this.SkyLight.Data[i] = 0xFF;
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
        }

        public void SetBlock(Vector3 position, byte value)
        {
            int x = (int)position.X;
            int y = (int)position.Y;
            int z = (int)position.Z;
            int index = x + (z * Width) + (y * Height * Width);
            this.Blocks[index] = value;
            this.Metadata[index] = 0;
            this.BlockLight[index] = 0;
            this.SkyLight[index] = 0xF;
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

