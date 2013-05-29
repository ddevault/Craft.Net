using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(FarmlandBlock.BlockId, FarmlandBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(FarmlandBlock.BlockId, DisplayName = FarmlandBlock.DisplayName, Hardness = FarmlandBlock.Hardness)]
    public static class FarmlandBlock
    {
        public const string DisplayName = "Farmland";
        public const short BlockId = 60;
		public const double Hardness = 0.6;
    }
}
