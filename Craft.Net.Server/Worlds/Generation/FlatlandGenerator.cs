using System;
using Craft.Net.Server.Blocks;

namespace Craft.Net.Server.Worlds.Generation
{
    public class FlatlandGenerator : IWorldGenerator
    {
        public FlatlandGenerator()
        {
        }

        public Chunk CreateChunk(Vector3 Position)
        {
            Chunk chunk = new Chunk();
            for (int x = 0; x < 16; x++)
            {
                for (int z = 0; z < 16; z++)
                {
                    for (int y = 1; y < 15; y++)
                        chunk.SetBlock(new Vector3(x, y, z), 3); // Dirt
                    chunk.SetBlock(new Vector3(x, 0, z), 7); // Bedrock
                    chunk.SetBlock(new Vector3(x, 15, z), 2); // Grass
                }
            }
            return chunk;
        }

        public string LevelType
        {
            get
            {
                return "FLAT";
            }
        }

        public double Seed { get; set; }

        public Vector3 SpawnPoint
        {
            get
            {
                return new Vector3(0, 16, 0);
            }
        }
    }
}

