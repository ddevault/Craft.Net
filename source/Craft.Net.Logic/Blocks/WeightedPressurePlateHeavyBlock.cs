using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(WeightedPressurePlateHeavyBlock.BlockId, WeightedPressurePlateHeavyBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(WeightedPressurePlateHeavyBlock.BlockId, DisplayName = WeightedPressurePlateHeavyBlock.DisplayName, Hardness = WeightedPressurePlateHeavyBlock.Hardness)]
    public static class WeightedPressurePlateHeavyBlock
    {
        public const string DisplayName = "Weighted Pressure Plate (Heavy)";
        public const short BlockId = 148;
        public const double Hardness = 0.5;
    }
}
