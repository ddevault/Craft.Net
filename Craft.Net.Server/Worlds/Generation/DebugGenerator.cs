using Craft.Net.Server.Blocks;

namespace Craft.Net.Server.Worlds.Generation
{
    /// <summary>
    /// A world generator where the generated terrain provides information
    /// about itself.
    /// </summary>
    public class DebugGenerator : IWorldGenerator
    {
        #region IWorldGenerator Members

        public string LevelType
        {
            get { return "FLAT"; }
        }

        public long Seed { get; set; }

        public Vector3 SpawnPoint
        {
            get { return new Vector3(0, 1, 1); }
        }

        public Chunk GenerateChunk(Vector3 Position, Region ParentRegion)
        {
            var chunk = new Chunk(Position, ParentRegion);

            for (int y = 0; y < Chunk.Height; y++)
            {
                if (y%Section.Height == 0)
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

        public Chunk GenerateChunk(Vector3 Position)
        {
            var chunk = new Chunk(Position);
            for (int x = 0; x < Chunk.Width; x++)
                for (int z = 0; z < Chunk.Width; z++)
                {
                    chunk.SetBlock(new Vector3(x, 0, z), new GoldBlock());
                }
            return chunk;
        }

        #endregion
    }
}