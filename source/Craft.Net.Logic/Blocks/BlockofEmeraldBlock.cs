using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(BlockofEmeraldBlock.BlockId, BlockofEmeraldBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(BlockofEmeraldBlock.BlockId, DisplayName = BlockofEmeraldBlock.DisplayName, Hardness = BlockofEmeraldBlock.Hardness)]
    public static class BlockofEmeraldBlock
    {
        public const string DisplayName = "Block of Emerald";
        public const short BlockId = 133;
		public const double Hardness = 5;
    }
}
