using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class FarmlandBlock : Block
    {
        public override short Id
        {
            get { return 60; }
        }

        public override double Hardness
        {
            get { return 0.6; }
        }

        public override bool GetDrop(Items.ToolItem tool, out ItemStack[] drop)
        {
            drop = new[] { new ItemStack(new DirtBlock(), 1) };
            return true;
        }

        public override void BlockUpdate(World world, Vector3 updatedBlock, Vector3 modifiedBlock)
        {
            if (!(world.GetBlock(updatedBlock + Vector3.Up) is AirBlock))
                world.SetBlock(updatedBlock, new DirtBlock());
            base.BlockUpdate(world, updatedBlock, modifiedBlock);
        }

        public override void OnBlockWalkedOn(World world, Vector3 position, Entities.Entity entity)
        {
            if (entity.Velocity.Y < -0.25)
                Console.WriteLine("Entity " + entity.Id + " jumped onto farmland with velocity " + entity.Velocity);
        }
    }
}
