using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(CropsBlock.BlockId, CropsBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(CropsBlock.BlockId, DisplayName = CropsBlock.DisplayName, Hardness = CropsBlock.Hardness)]
    public static class CropsBlock
    {
        public const string DisplayName = "Crops";
        public const short BlockId = 59;
		public const double Hardness = 0;
    }
}
