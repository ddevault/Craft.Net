using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(SpruceWoodStairsBlock.BlockId, SpruceWoodStairsBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(SpruceWoodStairsBlock.BlockId, DisplayName = SpruceWoodStairsBlock.DisplayName, Hardness = SpruceWoodStairsBlock.Hardness)]
    public static class SpruceWoodStairsBlock
    {
        public const string DisplayName = "Spruce Wood Stairs";
        public const short BlockId = 134;
        public const double Hardness = 0;
    }
}
