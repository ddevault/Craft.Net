using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(CobblestoneBlock.BlockId, CobblestoneBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(CobblestoneBlock.BlockId, DisplayName = CobblestoneBlock.DisplayName, Hardness = CobblestoneBlock.Hardness)]
    public static class CobblestoneBlock
    {
        public const string DisplayName = "Cobblestone";
        public const short BlockId = 4;
		public const double Hardness = 2;
    }
}
