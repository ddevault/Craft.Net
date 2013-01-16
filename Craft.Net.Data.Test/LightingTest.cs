using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Generation;
using NUnit.Framework;

namespace Craft.Net.Data.Test
{
    [TestFixture]
    public class LightingTest
    {
        [Test]
        public void TestSkyLight()
        {
            World world = new World(null, new TestWorldGenerator());
            world.GetChunk(new Vector3(0, 0, 0));
        }

        private class TestWorldGenerator : IWorldGenerator
        {
            public string LevelType
            {
                get { return "FLAT"; }
            }

            public string GeneratorName
            {
                get { return "FLAT"; }
            }

            public string GeneratorOptions { get; set; }

            public long Seed { get; set; }

            public Vector3 SpawnPoint
            {
                get { return new Vector3(0, 4, 0); }
            }

            public Chunk GenerateChunk(Vector3 position, Region parentRegion)
            {
                var chunk = GenerateChunk(position);
                chunk.ParentRegion = parentRegion;
                return chunk;
            }

            public Chunk GenerateChunk(Vector3 position)
            {
                var chunk = new Chunk(position);
                return chunk;
            }

            public void Initialize(Level level)
            {
            }
        }
    }
}
