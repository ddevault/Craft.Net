using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class WoodenDoorBlock : DoorBlock
    {
        public override ushort Id
        {
            get { return 64; }
        }

        public override bool OnBlockRightClicked(Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, World world, Entities.Entity usedBy)
        {
            if (!UpperHalf)
            {
                Closed = !Closed;
                world.SetBlock(clickedBlock, this);
            }
            else
            {
                DoorBlock block = (DoorBlock)world.GetBlock(clickedBlock + Vector3.Down);
                block.Closed = !block.Closed;
                world.SetBlock(clickedBlock + Vector3.Down, block);
            }
            return false;
        }

        public WoodenDoorBlock()
        {
        }

        public WoodenDoorBlock(DoorDirection direction, bool upperHalf)
        {
            UpperHalf = upperHalf;
            if (!UpperHalf)
                Direction = direction;
        }
    }
}
