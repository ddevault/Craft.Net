using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(GravelBlock.BlockId, GravelBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(GravelBlock.BlockId, DisplayName = GravelBlock.DisplayName, Hardness = GravelBlock.Hardness)]
    public static class GravelBlock
    {
        public const string DisplayName = "Gravel";
        public const short BlockId = 13;
		public const double Hardness = 0.6;
    }
}
