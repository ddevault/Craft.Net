using LibNbt;
using LibNbt.Tags;

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
        /// Creates a new Chunk within the specified <see cref="Craft.Net.Data.Region"/>
        /// at the specified location.
        /// </summary>
        public Chunk(Vector3 relativePosition, Region parentRegion)
        {
            Sections = new Section[16];
            for (int i = 0; i < Sections.Length; i++)
                Sections[i] = new Section((byte)i);
            RelativePosition = relativePosition;
            Biomes = new byte[Width * Depth];
            HeightMap = new byte[Width * Depth];
            ParentRegion = parentRegion;
        }

        /// <summary>
        /// Creates a new chunk at the specified location.
        /// </summary>
        public Chunk(Vector3 relativePosition)
        {
            Sections = new Section[16];
            for (int i = 0; i < Sections.Length; i++)
                Sections[i] = new Section((byte)i);
            RelativePosition = relativePosition;
            Biomes = new byte[Width * Depth];
            HeightMap = new byte[Width * Depth];
        }

        /// <summary>
        /// The location of this chunk in global,
        /// world-wide coordinates.
        /// </summary>
        public Vector3 AbsolutePosition 
        {
            get { return (ParentRegion.Position * new Vector3(Region.Width, 0, Region.Depth)) + RelativePosition; }
        }

        /// <summary>
        /// The biome data for this chunk.
        /// </summary>
        public byte[] Biomes { get; set; }

        /// <summary>
        /// Each byte corresponds to the height of the given 1x256x1
        /// column of blocks.
        /// </summary>
        public byte[] HeightMap { get; set; }

        /// <summary>
        /// The region this chunk is contained in.
        /// </summary>
        public Region ParentRegion { get; set; }

        /// <summary>
        /// The position of this chunk within the parent region.
        /// </summary>
        public Vector3 RelativePosition { get; set; }

        /// <summary>
        /// The 16x16x16 sections that make up this chunk.
        /// </summary>
        public Section[] Sections { get; set; }

        /// <summary>
        /// Sets the value of the block at the given position, relative to this chunk.
        /// </summary>
        public void SetBlock(Vector3 position, Block value)
        {
            var y = (byte)position.Y;
            y /= 16;
            position.Y = position.Y % 16;
            Sections[y].SetBlock(position, value);
            var heightIndex = (byte)(position.Z * Depth) + (byte)position.X;
            if (HeightMap[heightIndex] < position.Y)
                HeightMap[heightIndex] = (byte)position.Y;
        }

        /// <summary>
        /// Gets the block at the given position, relative to this chunk.
        /// </summary>
        public Block GetBlock(Vector3 position)
        {
            var y = (byte)position.Y;
            y /= 16;
            position.Y = position.Y % 16;
            return Sections[y].GetBlock(position);
        }

        /// <summary>
        /// Gets the biome at the given column.
        /// </summary>
        public Biome GetBiome(byte x, byte z)
        {
            return (Biome)Biomes[(byte)(z * Depth) + x];
        }

        /// <summary>
        /// Sets the value of the biome at the given column.
        /// </summary>
        public void SetBiome(byte x, byte z, Biome value)
        {
            Biomes[(byte)(z * Depth) + x] = (byte)value;
        }

        /// <summary>
        /// Gets the height of the specified column.
        /// </summary>
        public byte GetHeight(byte x, byte z)
        {
            return HeightMap[(byte)(z * Depth) + x];
        }

        public static Chunk FromNbt(Vector3 position, NbtFile nbt)
        {
            Chunk chunk = new Chunk(position);
            // Load data
            var root = nbt.RootTag.Get<NbtCompound>("Level");
            chunk.Biomes = root.Get<NbtByteArray>("Biomes").Value;
            int[] heightMap = root.Get<NbtIntArray>("HeightMap").Value;
            for (int i = 0; i < chunk.HeightMap.Length; i++ )
                chunk.HeightMap[i] = (byte)heightMap[i];
            var sections = root.Get<NbtList>("Sections");
            foreach (var sectionTag in sections.Tags)
            {
                // Load data
                var compound = (NbtCompound)sectionTag;
                byte y = compound.Get<NbtByte>("Y").Value;
                var section = new Section(y);
                section.Blocks = compound.Get<NbtByteArray>("Blocks").Value;
                section.BlockLight.Data = compound.Get<NbtByteArray>("BlockLight").Value;
                section.SkyLight.Data = compound.Get<NbtByteArray>("SkyLight").Value;
                section.Metadata.Data = compound.Get<NbtByteArray>("Data").Value;
                // Process section
                section.ProcessSection();
                chunk.Sections[y] = section;
            }
            return chunk;
        }
    }
}