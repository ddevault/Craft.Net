using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(WoodBlock.BlockId, WoodBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(WoodBlock.BlockId, DisplayName = WoodBlock.DisplayName, Hardness = WoodBlock.Hardness)]
    public static class WoodBlock
    {
        public const string DisplayName = "Wood";
        public const short BlockId = 17;
		public const double Hardness = 2;
    }
}
