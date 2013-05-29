using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(IronBarsBlock.BlockId, IronBarsBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(IronBarsBlock.BlockId, DisplayName = IronBarsBlock.DisplayName, Hardness = IronBarsBlock.Hardness)]
    public static class IronBarsBlock
    {
        public const string DisplayName = "Iron Bars";
        public const short BlockId = 101;
		public const double Hardness = 5;
    }
}
