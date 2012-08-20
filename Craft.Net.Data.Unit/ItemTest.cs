using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Blocks;
using Craft.Net.Data.Generation;
using Craft.Net.Data.Items;
using NUnit.Framework;

namespace Craft.Net.Data.Unit
{
    [TestFixture]
    public class ItemTest
    {
        public World World;

        [TestFixtureSetUp]
        public void SetUp()
        {
            World = new World(new FlatlandGenerator());
        }

        [Test]
        public void TestHoe()
        {
            Vector3 grassBlock = new Vector3(0, 3, 0);

            var hoe = new DiamondHoeItem();
            hoe.OnItemUsed(grassBlock, Vector3.Up, Vector3.Zero, World, null);

            Assert.AreEqual(World.GetBlock(grassBlock), new FarmlandBlock());
        }
    }
}
