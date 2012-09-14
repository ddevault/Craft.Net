using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Craft.Net.Data.Blocks;
using Craft.Net.Data.Entities;
using Craft.Net.Data.Generation;
using Craft.Net.Data.Items;
using NUnit.Framework;

namespace Craft.Net.Data.Test
{
    [TestFixture]
    public class ItemTest
    {
        [Test]
        public void TestItemList()
        {
            // Test order
            for (int i = 1; i < Item.Items.Count; i++)
                Assert.Less(Item.Items[i - 1].Id, Item.Items[i].Id);
            // Test completeness
            var itemTypes = Assembly.GetAssembly(typeof(Item)).GetTypes().Where(t =>
                !t.IsAbstract && typeof(Item).IsAssignableFrom(t)).OrderBy(t => ((Item)Activator.CreateInstance(t)).Id);
            string values = "";
            // Tests for completeness and generates passing code in case of failure
            foreach (var type in itemTypes)
                values += "new " + type.Name + "(),\n";
            foreach (var type in itemTypes)
            {
                var instance = (Item)Activator.CreateInstance(type);
                Assert.IsTrue(Item.Items.Count(i => i.Id == instance.Id) != 0);
            }
        }

        [Test]
        public void TestBuckets()
        {
            World world = new World(new FlatlandGenerator());
            world.WorldGenerator.Initialize(new Level());
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
            waterBucket.OnItemUsed(world, targetBlock, Vector3.Up, Vector3.Zero, player);
            Assert.AreEqual(new WaterFlowingBlock(), world.GetBlock(alteredBlock));

            bucket.OnItemUsed(world, targetBlock, Vector3.Up, Vector3.Zero, player);
            Assert.AreEqual(new AirBlock(), world.GetBlock(alteredBlock));

            lavaBucket.OnItemUsed(world, targetBlock, Vector3.Up, Vector3.Zero, player);
            Assert.AreEqual(new LavaFlowingBlock(), world.GetBlock(alteredBlock));

            bucket.OnItemUsed(world, targetBlock, Vector3.Up, Vector3.Zero, player);
            Assert.AreEqual(new AirBlock(), world.GetBlock(alteredBlock));

            world.SetBlock(alteredBlock, new BedrockBlock());

            bucket.OnItemUsed(world, targetBlock, Vector3.Up, Vector3.Zero, player);
            Assert.AreEqual(new BedrockBlock(), world.GetBlock(alteredBlock));
        }

        [Test]
        public void TestFlintAndSteel()
        {
            World world = new World(new FlatlandGenerator());
            world.WorldGenerator.Initialize(new Level());
            Vector3 targetBlock = new Vector3(0, 3, 0);
            Vector3 alteredBlock = targetBlock + Vector3.Up;

            FlintAndSteelItem flintAndSteel = new FlintAndSteelItem();
            flintAndSteel.OnItemUsed(world, targetBlock, Vector3.Up, Vector3.Zero, null);

            Assert.AreEqual(new FireBlock(), world.GetBlock(alteredBlock));
        }

        [Test]
        public void TestBrewingStand()
        {
            World world = new World(new FlatlandGenerator());
            world.WorldGenerator.Initialize(new Level());

            BrewingStandItem brewingStand = new BrewingStandItem();
            brewingStand.OnItemUsed(world, new Vector3(0, 3, 0), Vector3.Up, Vector3.Zero, null);

            Assert.AreEqual(new BrewingStandBlock(), world.GetBlock(new Vector3(0, 4, 0)));
        }

        [Test]
        public void TestHoe()
        {
            World world = new World(new FlatlandGenerator());
            world.WorldGenerator.Initialize(new Level());
            Vector3 grassBlock = new Vector3(0, 3, 0);

            HoeItem hoe = new DiamondHoeItem();
            hoe.OnItemUsed(world, grassBlock, Vector3.Up, Vector3.Zero, null);

            grassBlock.X++;
            hoe = new IronHoeItem();
            hoe.OnItemUsed(world, grassBlock, Vector3.Up, Vector3.Zero, null);

            grassBlock.X++;
            hoe = new GoldenHoeItem();
            hoe.OnItemUsed(world, grassBlock, Vector3.Up, Vector3.Zero, null);

            grassBlock.X++;
            hoe = new StoneHoeItem();
            hoe.OnItemUsed(world, grassBlock, Vector3.Up, Vector3.Zero, null);

            grassBlock.X++;
            hoe = new WoodenHoeItem();
            hoe.OnItemUsed(world, grassBlock, Vector3.Up, Vector3.Zero, null);

            var testBlock = new Vector3(0, 3, 0);

            while (testBlock != grassBlock)
            {
                Assert.AreEqual(new FarmlandBlock(), world.GetBlock(testBlock));
                testBlock.X++;
            }
        }
    }
}
