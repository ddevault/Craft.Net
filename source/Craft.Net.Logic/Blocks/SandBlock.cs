using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(SandBlock.BlockId, SandBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(SandBlock.BlockId, DisplayName = SandBlock.DisplayName, Hardness = SandBlock.Hardness)]
    public static class SandBlock
    {
        public const string DisplayName = "Sand";
        public const short BlockId = 12;
		public const double Hardness = 0.5;
    }
}
