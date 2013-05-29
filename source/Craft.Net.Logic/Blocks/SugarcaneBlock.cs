using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(SugarcaneBlock.BlockId, SugarcaneBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(SugarcaneBlock.BlockId, DisplayName = SugarcaneBlock.DisplayName, Hardness = SugarcaneBlock.Hardness)]
    public static class SugarcaneBlock
    {
        public const string DisplayName = "Sugar cane";
        public const short BlockId = 83;
		public const double Hardness = 0;
    }
}
