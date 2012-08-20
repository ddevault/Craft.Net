using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Blocks;
using Craft.Net.Data.Entities;
using Craft.Net.Data.Generation;
using Craft.Net.Data.Items;
using NUnit.Framework;

namespace Craft.Net.Data.Unit
{
    [TestFixture]
    public class ItemTest
    {
        [Test]
        public void TestBuckets()
        {
            World world = new World(new FlatlandGenerator());
            PlayerEntity player = new PlayerEntity()
            {
                GameMode = GameMode.Creative
            };
            Vector3 targetBlock = new Vector3(0, 2, 0);
            Vector3 alteredBlock = targetBlock + Vector3.Up;
            world.SetBlock(alteredBlock, new AirBlock());

            BucketItem bucket = new BucketItem();
            LavaBucketItem lavaBucket = new LavaBucketItem();
            WaterBucketItem waterBucket = new WaterBucketItem();

            // TODO: Survival tests
            waterBucket.OnItemUsed(targetBlock, Vector3.Up, Vector3.Zero, world, player);
            Assert.AreEqual(new WaterFlowingBlock(), world.GetBlock(alteredBlock));

            bucket.OnItemUsed(targetBlock, Vector3.Up, Vector3.Zero, world, player);
            Assert.AreEqual(new AirBlock(), world.GetBlock(alteredBlock));

            lavaBucket.OnItemUsed(targetBlock, Vector3.Up, Vector3.Zero, world, player);
            Assert.AreEqual(new LavaFlowingBlock(), world.GetBlock(alteredBlock));

            bucket.OnItemUsed(targetBlock, Vector3.Up, Vector3.Zero, world, player);
            Assert.AreEqual(new AirBlock(), world.GetBlock(alteredBlock));

            world.SetBlock(alteredBlock, new BedrockBlock());

            bucket.OnItemUsed(targetBlock, Vector3.Up, Vector3.Zero, world, player);
            Assert.AreEqual(new BedrockBlock(), world.GetBlock(alteredBlock));
        }

        [Test]
        public void TestFlintAndSteel()
        {
            World world = new World(new FlatlandGenerator());
            Vector3 targetBlock = new Vector3(0, 3, 0);
            Vector3 alteredBlock = targetBlock + Vector3.Up;

            FlintAndSteelItem flintAndSteel = new FlintAndSteelItem();
            flintAndSteel.OnItemUsed(targetBlock, Vector3.Up, Vector3.Zero, world, null);

            Assert.AreEqual(new FireBlock(), world.GetBlock(alteredBlock));
        }

        [Test]
        public void TestBrewingStand()
        {
            World world = new World(new FlatlandGenerator());

            BrewingStandItem brewingStand = new BrewingStandItem();
            brewingStand.OnItemUsed(new Vector3(0, 3, 0), Vector3.Up, Vector3.Zero, world, null);

            Assert.AreEqual(new BrewingStandBlock(), world.GetBlock(new Vector3(0, 4, 0)));
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
                Assert.AreEqual(new FarmlandBlock(), world.GetBlock(testBlock));
                testBlock.X++;
            }
        }
    }
}
