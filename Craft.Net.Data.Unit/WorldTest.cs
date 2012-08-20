using System;
using System.Collections.Generic;
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
            World = new World(new FlatlandGenerator());
        }

        [Test]
        public void TestGetBlock()
        {
            // Test vertical sections
            Assert.AreEqual(World.GetBlock(Vector3.Zero), new BedrockBlock());
            Assert.AreEqual(World.GetBlock(Vector3.Up), new DirtBlock());
            Assert.AreEqual(World.GetBlock(new Vector3(0, 3, 0)), new GrassBlock());

            // Test quadrants
            Assert.AreEqual(World.GetBlock(new Vector3(1, 0, 1)), new BedrockBlock());
            Assert.AreEqual(World.GetBlock(new Vector3(-1, 0, 1)), new BedrockBlock());
            Assert.AreEqual(World.GetBlock(new Vector3(1, 0, -1)), new BedrockBlock());
            Assert.AreEqual(World.GetBlock(new Vector3(-1, 0, -1)), new BedrockBlock());
        }

        [Test]
        public void TestSetBlock()
        {
            World.SetBlock(Vector3.Zero, new GoldBlock());
            Assert.AreEqual(World.GetBlock(Vector3.Zero), new GoldBlock());
        }
    }
}
