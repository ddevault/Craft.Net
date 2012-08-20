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
        [Test]
        public void TestBrewingStand()
        {
            World world = new World(new FlatlandGenerator());

            BrewingStandItem brewingStand = new BrewingStandItem();
            brewingStand.OnItemUsed(new Vector3(0, 3, 0), Vector3.Up, Vector3.Zero, world, null);

            Assert.AreEqual(world.GetBlock(new Vector3(0, 4, 0)), new BrewingStandBlock());
        }

        [Test]
        public void TestHoe()
        {
            World world = new World(new FlatlandGenerator());
            Vector3 grassBlock = new Vector3(0, 3, 0);

            HoeItem hoe = new DiamondHoeItem();
            hoe.OnItemUsed(grassBlock, Vector3.Up, Vector3.Zero, world, null);

            grassBlock.X++;
            hoe = new IronHoeItem();
            hoe.OnItemUsed(grassBlock, Vector3.Up, Vector3.Zero, world, null);

            grassBlock.X++;
            hoe = new GoldenHoeItem();
            hoe.OnItemUsed(grassBlock, Vector3.Up, Vector3.Zero, world, null);

            grassBlock.X++;
            hoe = new StoneHoeItem();
            hoe.OnItemUsed(grassBlock, Vector3.Up, Vector3.Zero, world, null);

            grassBlock.X++;
            hoe = new WoodenHoeItem();
            hoe.OnItemUsed(grassBlock, Vector3.Up, Vector3.Zero, world, null);

            var testBlock = new Vector3(0, 3, 0);

            while (testBlock != grassBlock)
            {
                Assert.AreEqual(world.GetBlock(testBlock), new FarmlandBlock());
                testBlock.X++;
            }
        }
    }
}
