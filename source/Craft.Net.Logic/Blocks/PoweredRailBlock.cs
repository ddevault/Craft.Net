using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(PoweredRailBlock.BlockId, PoweredRailBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(PoweredRailBlock.BlockId, DisplayName = PoweredRailBlock.DisplayName, Hardness = PoweredRailBlock.Hardness)]
    public static class PoweredRailBlock
    {
        public const string DisplayName = "Powered Rail";
        public const short BlockId = 27;
		public const double Hardness = 0.7;
    }
}
