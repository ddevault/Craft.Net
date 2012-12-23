using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
    public class WoodenDoorBlock : DoorBlock
    {
        public override short Id
        {
            get { return 64; }
        }

        public override double Hardness
        {
            get { return 3; }
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

        public override bool GetDrop(ToolItem tool, out Slot[] drop)
        {
            drop = new[] { new Slot(new WoodenDoorItem(), 1) };
            return UpperHalf;
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
