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
        }
    }
}
