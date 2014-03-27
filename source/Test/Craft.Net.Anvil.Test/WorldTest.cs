using Craft.Net.TerrainGeneration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Craft.Net.Common;

namespace Craft.Net.Anvil.Test
{
    [TestFixture]
    public class WorldTest
    {
        [Test]
        public void TestReading()
        {
            using (var level = Level.LoadFrom("Test World"))
            {
                Assert.AreEqual(0, level.DefaultWorld.GetBlockId(new Coordinates3D(0, 100, 0))); // Air
                Assert.AreEqual(20, level.DefaultWorld.GetBlockId(new Coordinates3D(0, 3, 0)));  // Glass
                Assert.AreEqual(22, level.DefaultWorld.GetBlockId(new Coordinates3D(-1, 3, 0))); // Lapis
                Assert.AreEqual(4, level.DefaultWorld.GetBlockId(new Coordinates3D(-1, 3, -1))); // Cobblestone
                Assert.AreEqual(24, level.DefaultWorld.GetBlockId(new Coordinates3D(0, 3, -1))); // Sandstone
            }
        }

        [Test]
        public void TestUnrecognizedEntities()
        {
            using (var level = Level.LoadFrom("Test World"))
            {
                var chunk = level.DefaultWorld.GetChunk(new Coordinates2D(0, 0));
                Assert.AreEqual(1, chunk.Entities.Count);
                Assert.AreEqual("Creeper", chunk.Entities[0].Id);
            }
        }

        [Test]
        public void TestBlockManipulation()
        {
            using (var level = new Level(new FlatlandGenerator(), "Example"))
            {
                level.AddWorld("region");
                level.DefaultWorld.SetBlockId(Coordinates3D.Zero, 22);
                Assert.AreEqual(22, level.DefaultWorld.GetBlockId(Coordinates3D.Zero));

                level.DefaultWorld.SetBlockId(new Coordinates3D(0, 0, -1), 22);
                Assert.AreEqual(22, level.DefaultWorld.GetBlockId(new Coordinates3D(0, 0, -1)));

                level.DefaultWorld.SetBlockId(new Coordinates3D(-1, 0, -1), 22);
                Assert.AreEqual(22, level.DefaultWorld.GetBlockId(new Coordinates3D(-1, 0, -1)));

                level.DefaultWorld.SetBlockId(new Coordinates3D(-1, 0, 0), 22);
                Assert.AreEqual(22, level.DefaultWorld.GetBlockId(new Coordinates3D(-1, 0, 0)));

                level.SaveTo("Example");
            }
        }
    }
}
