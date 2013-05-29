using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(PumpkinBlock.BlockId, PumpkinBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(PumpkinBlock.BlockId, DisplayName = PumpkinBlock.DisplayName, Hardness = PumpkinBlock.Hardness)]
    public static class PumpkinBlock
    {
        public const string DisplayName = "Pumpkin";
        public const short BlockId = 86;
		public const double Hardness = 1;
    }
}
