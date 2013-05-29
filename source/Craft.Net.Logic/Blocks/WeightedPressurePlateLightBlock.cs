using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(WeightedPressurePlateLightBlock.BlockId, WeightedPressurePlateLightBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(WeightedPressurePlateLightBlock.BlockId, DisplayName = WeightedPressurePlateLightBlock.DisplayName, Hardness = WeightedPressurePlateLightBlock.Hardness)]
    public static class WeightedPressurePlateLightBlock
    {
        public const string DisplayName = "Weighted Pressure Plate (Light)";
        public const short BlockId = 147;
		public const double Hardness = 0.5;
    }
}
