using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
    public class IronDoorBlock : DoorBlock
    {
        public override ushort Id
        {
            get { return 71; }
        }

        public override double Hardness
        {
            get { return 5; }
        }

        public override bool OnBlockRightClicked(Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, World world, Entities.Entity usedBy)
        {
            return false;
        }

        public IronDoorBlock()
        {
        }

        public IronDoorBlock(DoorDirection direction, bool upperHalf)
        {
            UpperHalf = upperHalf;
            if (!UpperHalf)
                Direction = direction;
        }

        public override bool CanHarvest(Items.ToolItem tool)
        {
            return tool is PickaxeItem || tool is AxeItem;
        }
    }
}
