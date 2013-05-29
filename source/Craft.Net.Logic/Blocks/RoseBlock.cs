using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(RoseBlock.BlockId, RoseBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(RoseBlock.BlockId, DisplayName = RoseBlock.DisplayName, Hardness = RoseBlock.Hardness)]
    public static class RoseBlock
    {
        public const string DisplayName = "Rose";
        public const short BlockId = 38;
		public const double Hardness = 0;
    }
}
