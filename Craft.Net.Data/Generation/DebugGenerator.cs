using Craft.Net.Data.Blocks;

namespace Craft.Net.Data.Generation
{
    /// <summary>
    /// A world generator where the generated terrain provides information
    /// about itself.
    /// </summary>
    public class DebugGenerator : IWorldGenerator
    {
        public string LevelType
        {
            get { return "FLAT"; }
        }

        public long Seed { get; set; }

        /// <summary>
        /// The spawn point is always set to &lt;0, 1, 1&gt;
        /// </summary>
        public Vector3 SpawnPoint
        {
            get { return new Vector3(0, 1, 1); }
        }

        /// <summary>
        /// Generates a chunk with red wool across the X axis, blue
        /// wool across the Z axis, and yellow wool across the Y axis,
        /// divided into sections by glass.
        /// </summary>
        public Chunk GenerateChunk(Vector3 position, Region parentRegion)
        {
            var chunk = new Chunk(position, parentRegion);

            for (int y = 0; y < Chunk.Height; y++)
            {
                if (y % Section.Height == 0)
                {
                    for (int x = 0; x < Chunk.Width; x++)
                    {
                        for (int z = 0; z < Chunk.Width; z++)
                            chunk.SetBlock(new Vector3(x, y, z), new GlassBlock());
                        chunk.SetBlock(new Vector3(x, y, 0), new WoolBlock(WoolColor.Red));
                    }
                    for (int z = 0; z < Chunk.Width; z++)
                        chunk.SetBlock(new Vector3(0, y, z), new WoolBlock(WoolColor.Blue));
                }
                chunk.SetBlock(new Vector3(0, y, 0), new WoolBlock(WoolColor.Yellow));
            }
            return chunk;
        }

        /// <summary>
        /// Generates a chunk with red wool across the X axis, blue
        /// wool across the Z axis, and yellow wool across the Y axis,
        /// divided into sections by glass.
        /// </summary>
        public Chunk GenerateChunk(Vector3 position)
        {
            var chunk = new Chunk(position);
            for (int x = 0; x < Chunk.Width; x++)
                for (int z = 0; z < Chunk.Width; z++)
                {
                    chunk.SetBlock(new Vector3(x, 0, z), new GoldBlock());
                }
            return chunk;
        }

        public string GeneratorName
        {
            get { return "DebugGenerator"; }
        }
    }
}