using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(ActivatorRailBlock.BlockId, ActivatorRailBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(ActivatorRailBlock.BlockId, DisplayName = ActivatorRailBlock.DisplayName, Hardness = ActivatorRailBlock.Hardness)]
    public static class ActivatorRailBlock
    {
        public const string DisplayName = "Activator Rail";
        public const short BlockId = 157;
		public const double Hardness = 0.7;
    }
}
