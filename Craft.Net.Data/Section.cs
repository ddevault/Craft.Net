using Craft.Net.Data.Blocks;
using ICSharpCode.SharpZipLib.Zip.Compression;

namespace Craft.Net.Data
{
    /// <summary>
    /// Represents a 16x16x16 section of blocks within a <see cref="Chunk"/>
    /// </summary>
    public class Section
    {
        public const byte Width = 16, Height = 16, Depth = 16;

        /// <summary>
        /// The raw block data in this section.
        /// </summary>
        public byte[] Blocks;
        /// <summary>
        /// The raw metadata in this section.
        /// </summary>
        public NibbleArray Metadata;
        /// <summary>
        /// The raw block light in this section.
        /// </summary>
        public NibbleArray BlockLight;
        /// <summary>
        /// The raw sky light in this section.
        /// </summary>
        public NibbleArray SkyLight;
        /// <summary>
        /// The Y location of this section, in relative coordinates.
        /// </summary>
        public byte Y;

        private int nonAirCount;

        /// <summary>
        /// Creates a new section at the given Y location.
        /// </summary>
        /// <param name="y">The location of the section in relative coordinates.</param>
        public Section(byte y)
        {
            this.Y = y;
            Blocks = new byte[Width*Height*Depth];
            Metadata = new NibbleArray(Width*Height*Depth);
            BlockLight = new NibbleArray(Width*Height*Depth);
            SkyLight = new NibbleArray(Width*Height*Depth);
            for (int i = 0; i < SkyLight.Data.Length; i++)
                SkyLight.Data[i] = BlockLight.Data[i] = 0xFF;
            nonAirCount = 0;
        }

        /// <summary>
        /// Gets a value indicating whether this chunk is completely air.
        /// </summary>
        public bool IsAir
        {
            get { return nonAirCount == 0; }
        }

        /// <summary>
        /// Sets the block at the given position in local coordinates.
        /// </summary>
        public void SetBlock(Vector3 position, Block value)
        {
            var x = (int)position.X;
            var y = (int)position.Y;
            var z = (int)position.Z;
            int index = x + (z*Width) + (y*Height*Width);
            Blocks[index] = value.BlockID;
            Metadata[index] = value.Metadata;
            BlockLight[index] = value.BlockLight;
            SkyLight[index] = value.SkyLight;
            if (value is AirBlock)
                nonAirCount--;
            else
                nonAirCount++;
        }

        /// <summary>
        /// Gets the block at the given position in local coordinates.
        /// </summary>
        public Block GetBlock(Vector3 position)
        {
            var x = (int)position.X;
            var y = (int)position.Y;
            var z = (int)position.Z;
            int index = x + (z*Width) + (y*Height*Width);
            Block block = Blocks[index];
            block.Metadata = Metadata[index];
            block.SkyLight = SkyLight[index];
            block.BlockLight = BlockLight[index];
            return block;
        }
    }
}