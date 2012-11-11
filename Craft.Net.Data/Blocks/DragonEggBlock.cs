using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Entities;

namespace Craft.Net.Data.Blocks
{
    public class DragonEggBlock : Block
    {
        public override ushort Id
        {
            get { return 122; }
        }

        public override double Hardness
        {
            get { return 3; }
        }

        public override bool OnBlockRightClicked(Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, World world, Entities.Entity usedBy)
        {
            return false;
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
