using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Data.Blocks;
using Craft.Net.Data.Generation;
using NUnit.Framework;

namespace Craft.Net.Data.Unit
{
    [TestFixture]
    public class WorldTest
    {
        public World World;

        [TestFixtureSetUp]
        public void SetUp()
        {
            World = new World(new FlatlandGenerator(), "world");
        }

        [Test]
        public void TestGetRegionFile()
        {
            Vector3 region = new Vector3(-2, 0, -2);
            Assert.AreEqual("r.-2.-2.mca", Region.GetRegionFileName(region));
            region = new Vector3(2, 0, 2);
            Assert.AreEqual("r.2.2.mca", Region.GetRegionFileName(region));
            region = new Vector3(0, 0, 0);
            Assert.AreEqual("r.0.0.mca", Region.GetRegionFileName(region));
        }

        [Test]
        public void TestLoadRegion()
        {
            Region region = new Region(Vector3.Zero, null, Path.Combine("TestWorld", "region", "r.0.0.mca"));
            var chunk = region.GetChunk(Vector3.Zero);
            for (int y = 0; y < Chunk.Height - 1; y++ )
                Assert.AreEqual(new GoldBlock(), chunk.GetBlock(new Vector3(0, y, 0)));
        }

        [Test]
        public void TestSaveRegion()
        {
            Region region = new Region(Vector3.Zero, new FlatlandGenerator(), "r.0.0.mca");
            region.GetChunk(Vector3.Zero);
            region.SetBlock(Vector3.Zero, new GoldBlock());
            region.Save();
            region.Dispose();
            region = new Region(Vector3.Zero, new FlatlandGenerator(), "r.0.0.mca");
        }

        [Test]
        public void TestGetBlock()
        {
            // Test vertical sections
            Assert.AreEqual(new BedrockBlock(), World.GetBlock(Vector3.Zero));
            Assert.AreEqual(new DirtBlock(), World.GetBlock(Vector3.Up));
            Assert.AreEqual(new GrassBlock(), World.GetBlock(new Vector3(0, 3, 0)));

            // Test quadrants
            Assert.AreEqual(new BedrockBlock(), World.GetBlock(new Vector3(1, 0, 1)));
            Assert.AreEqual(new BedrockBlock(), World.GetBlock(new Vector3(-1, 0, 1)));
            Assert.AreEqual(new BedrockBlock(), World.GetBlock(new Vector3(1, 0, -1)));
            Assert.AreEqual(new BedrockBlock(), World.GetBlock(new Vector3(-1, 0, -1)));
        }

        [Test]
        public void TestSetBlock()
        {
            World.SetBlock(Vector3.Zero, new GoldBlock());
            Assert.AreEqual(new GoldBlock(), World.GetBlock(Vector3.Zero));
            World.SetBlock(new Vector3(-15, 0, 5), new GoldBlock());
            Assert.AreEqual(new GoldBlock(), World.GetBlock(new Vector3(-15, 0, 5)));
        }
    }
}
