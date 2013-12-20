using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Common;
using Craft.Net.Logic.Items;

namespace Craft.Net.Logic.Blocks
{
    [Item(CakeBlock.BlockId, CakeBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(CakeBlock.BlockId, CakeBlock.DisplayName, "Initialize")]
    public static class CakeBlock
    {
        public const string DisplayName = "Cake";
        public const short BlockId = 92;

        public static BlockLogicDescriptor Initialize(BlockLogicDescriptor descriptor)
        {
            descriptor.GetSupportDirection = (b, w, c) => SupportDirection.Down;
            descriptor.GetDrop = (b, w, c) => new[] { new ItemStack(CakeItem.ItemId) };
            descriptor.BlockRightClicked = (block, world, coord, side, cursor) =>
                {
                    byte data = world.GetMetadata(coord);
                    if (data == 0x5)
                    {
                        world.SetBlock(coord, new BlockDescriptor(AirBlock.BlockId)); // If there is no more pieces left then delete block
                        return true;
                    }
                    else
                    {
                        data++;
                        world.SetMetadata(coord, data); // Eat a piece
                        return true;
                    }
                };
            return descriptor;
        }
    }
}
