using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(BlockofRedstoneBlock.BlockId, BlockofRedstoneBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(BlockofRedstoneBlock.BlockId, DisplayName = BlockofRedstoneBlock.DisplayName, Hardness = BlockofRedstoneBlock.Hardness)]
    public static class BlockofRedstoneBlock
    {
        public const string DisplayName = "Block of Redstone";
        public const short BlockId = 152;
		public const double Hardness = 5;
    }
}
