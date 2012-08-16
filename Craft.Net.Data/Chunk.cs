using Craft.Net.Data.Blocks;

namespace Craft.Net.Data
{
    /// <summary>
    /// Used to represent a 16x256x16 chunk of blocks in
    /// a <see cref="Craft.Net.Data.World"/>.
    /// </summary>
    public class Chunk
    {
        public const int Width = 16, Height = 256, Depth = 16;

        /// <summary>
        /// The location of this chunk in global,
        /// world-wide coordinates.
        /// </summary>
        public Vector3 AbsolutePosition;
        /// <summary>
        /// The biome data for this chunk.
        /// </summary>
        public byte[] Biomes;
        /// <summary>
        /// The region this chunk is contained in.
        /// </summary>
        public Region ParentRegion;
        /// <summary>
        /// The position of this chunk within the parent region.
        /// </summary>
        public Vector3 RelativePosition;
        /// <summary>
        /// The 16x16x16 sections that make up this chunk.
        /// </summary>
        public Section[] Sections;

        /// <summary>
        /// Creates a new Chunk within the specified <see cref="Craft.Net.Data.Region"/>
        /// at the specified location.
        /// </summary>
        public Chunk(Vector3 relativePosition, Region parentRegion)
        {
            Sections = new Section[16];
            for (int i = 0; i < Sections.Length; i++)
                Sections[i] = new Section((byte)i);
            this.RelativePosition = relativePosition;
            Biomes = new byte[Width*Depth];
            this.ParentRegion = parentRegion;
            AbsolutePosition =
                (parentRegion.Position*new Vector3(Region.Width, 0, Region.Depth))
                + relativePosition;
        }

        /// <summary>
        /// Creates a new chunk at the specified location.
        /// </summary>
        public Chunk(Vector3 relativePosition)
        {
            Sections = new Section[16];
            for (int i = 0; i < Sections.Length; i++)
                Sections[i] = new Section((byte)i);
            this.RelativePosition = relativePosition;
            Biomes = new byte[Width*Depth];
        }

        /// <summary>
        /// Sets the value of the block at the given position, relative to this chunk.
        /// </summary>
        public void SetBlock(Vector3 position, Block value)
        {
            var y = (byte)position.Y;
            y /= 16;
            position.Y = position.Y%16;
            Sections[y].SetBlock(position, value);
        }

        /// <summary>
        /// Gets the block at the given position, relative to this chunk.
        /// </summary>
        public Block GetBlock(Vector3 position)
        {
            var y = (byte)position.Y;
            y /= 16;
            position.Y = position.Y%16;
            return Sections[y].GetBlock(position);
        }
    }
}