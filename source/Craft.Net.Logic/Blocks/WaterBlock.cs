using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(WaterBlock.BlockId, WaterBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(WaterBlock.BlockId, DisplayName = WaterBlock.DisplayName, Hardness = WaterBlock.Hardness)]
    public static class WaterBlock
    {
        public const string DisplayName = "Water";
        public const short BlockId = 8;
		public const double Hardness = 100;
    }
}
