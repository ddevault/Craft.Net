using Craft.Net.Anvil;
using Craft.Net.Logic.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(CakeItem.ItemId, "Initialize")]
    public static class CakeItem
    {
        public const short ItemId = 354;

        public static void Initialize(Item.ItemDescriptor descriptor)
        {
            descriptor.ItemUsedOnBlock = OnItemUsedOnBlock;
        }

        public static void OnItemUsedOnBlock(World world, Coordinates3D clickedBlock, Coordinates3D clickedSide, Coordinates2D cursorPosition)
        {
            if (world.GetBlockId(clickedBlock + clickedSide) == 0)
                world.SetBlockId(clickedBlock + clickedSide, CakeBlock.BlockId);
        }
    }
}
