using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(PumpkinStemBlock.BlockId, PumpkinStemBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(PumpkinStemBlock.BlockId, DisplayName = PumpkinStemBlock.DisplayName, Hardness = PumpkinStemBlock.Hardness)]
    public static class PumpkinStemBlock
    {
        public const string DisplayName = "PumpkinStem";
        public const short BlockId = 104;
		public const double Hardness = 0;
    }
}
