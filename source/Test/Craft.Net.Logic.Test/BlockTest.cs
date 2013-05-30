using Craft.Net.Anvil;
using Craft.Net.Logic.Blocks;
using Craft.Net.Logic.Items;
using Craft.Net.TerrainGeneration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Logic.Test
{
    [TestFixture]
    public class BlockTest
    {
        [Test]
        public void TestCakeBlock()
        {
            using (var level = new Level(new FlatlandGenerator()))
            {
                level.AddWorld("overworld");
                Item.OnItemUsedOnBlock(new ItemDescriptor(CakeBlock.BlockId), level.DefaultWorld, new Coordinates3D(0, 3, 0),
                    Coordinates3D.Up, Coordinates3D.Zero);
                Assert.AreEqual(CakeBlock.BlockId, level.DefaultWorld.GetBlockId(new Coordinates3D(0, 4, 0)));
            }
        }
    }
}
