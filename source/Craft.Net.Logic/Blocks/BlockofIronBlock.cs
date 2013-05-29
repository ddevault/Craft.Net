using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(BlockofIronBlock.BlockId, BlockofIronBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(BlockofIronBlock.BlockId, DisplayName = BlockofIronBlock.DisplayName, Hardness = BlockofIronBlock.Hardness)]
    public static class BlockofIronBlock
    {
        public const string DisplayName = "Block of Iron";
        public const short BlockId = 42;
		public const double Hardness = 5;
    }
}
