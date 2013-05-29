using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(SoulSandBlock.BlockId, SoulSandBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(SoulSandBlock.BlockId, DisplayName = SoulSandBlock.DisplayName, Hardness = SoulSandBlock.Hardness)]
    public static class SoulSandBlock
    {
        public const string DisplayName = "Soul Sand";
        public const short BlockId = 88;
		public const double Hardness = 0.5;
    }
}
