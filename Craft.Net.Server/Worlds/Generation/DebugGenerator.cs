using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Server.Blocks;

namespace Craft.Net.Server.Worlds.Generation
{
    /// <summary>
    /// A world generator where the generated terrain provides information
    /// about itself.
    /// </summary>
    public class DebugGenerator : IWorldGenerator
    {
        public string LevelType
        {
            get { return "DEBUG"; }
        }

        public long Seed { get; set; }

        public Vector3 SpawnPoint
        {
            get { return new Vector3(0, 1, 0); }
        }

        public Chunk GenerateChunk(Vector3 Position, Region ParentRegion)
        {
            Chunk chunk = new Chunk(Position, ParentRegion);
            for (int x = 0; x < Chunk.Width; x++)
                for (int z = 0; z < Chunk.Width; z++)
                {
                    chunk.SetBlock(new Vector3(x, 0, z), new GoldBlock());
                }
            return chunk;
        }

        public Chunk GenerateChunk(Vector3 Position)
        {
            Chunk chunk = new Chunk(Position);
            for (int x = 0; x < Chunk.Width; x++)
                for (int z = 0; z < Chunk.Width; z++)
                {
                    chunk.SetBlock(new Vector3(x, 0, z), new GoldBlock());
                }
            return chunk;
        }
    }
}
