using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;
using Craft.Net.Logic.Blocks;

namespace Craft.Net.Logic.Items
{
    [Item(CakeItem.ItemId, CakeItem.DisplayName, "Initialize")]
    public static class CakeItem
    {
        public const short ItemId = 354;
        public const string DisplayName = "Cake";

        public static void Initialize(ItemLogicDescriptor descriptor)
        {
            descriptor.ItemUsedOnBlock = OnItemUsedOnBlock;
        }

        public static void OnItemUsedOnBlock(ItemDescriptor item, World world, Coordinates3D clickedBlock, Coordinates3D clickedSide, Coordinates2D cursorPosition)
        {
            if (world.GetBlockId(clickedBlock + clickedSide) == 0)
                world.SetBlockId(clickedBlock + clickedSide, CakeBlock.BlockId);
        }
    }
}
