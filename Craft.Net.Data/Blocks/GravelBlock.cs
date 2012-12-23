using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Entities;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
    public class GravelBlock : Block
    {
        public override short Id
        {
            get { return 13; }
        }

        public override double Hardness
        {
            get { return 0.6; }
        }

        public override bool GetDrop(ToolItem tool, out Slot[] drop)
        {
            if (MathHelper.Random.Next(10) == 0)
            {
                drop = new[] { new Slot(new FlintItem(), 1) };
                return true;
            }
            return base.GetDrop(tool, out drop);
        }

        public override void BlockUpdate(World world, Vector3 updatedBlock, Vector3 modifiedBlock)
        {
            if (world.GetBlock(updatedBlock + Vector3.Down) == 0)
            {
                world.SetBlock(updatedBlock, new AirBlock());
                world.OnSpawnEntity(new BlockEntity(updatedBlock, this));
            }
        }
    }
}
