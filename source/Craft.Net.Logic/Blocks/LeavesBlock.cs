using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(LeavesBlock.BlockId, LeavesBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(LeavesBlock.BlockId, DisplayName = LeavesBlock.DisplayName, Hardness = LeavesBlock.Hardness)]
    public static class LeavesBlock
    {
        public const string DisplayName = "Leaves";
        public const short BlockId = 18;
		public const double Hardness = 0.2;
    }
}
