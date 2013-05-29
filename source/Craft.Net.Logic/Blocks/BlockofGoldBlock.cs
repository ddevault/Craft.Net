using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(BlockofGoldBlock.BlockId, BlockofGoldBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(BlockofGoldBlock.BlockId, DisplayName = BlockofGoldBlock.DisplayName, Hardness = BlockofGoldBlock.Hardness)]
    public static class BlockofGoldBlock
    {
        public const string DisplayName = "Block of Gold";
        public const short BlockId = 41;
		public const double Hardness = 3;
    }
}
