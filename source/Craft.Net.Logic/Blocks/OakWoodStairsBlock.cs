using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(OakWoodStairsBlock.BlockId, OakWoodStairsBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(OakWoodStairsBlock.BlockId, DisplayName = OakWoodStairsBlock.DisplayName, Hardness = OakWoodStairsBlock.Hardness)]
    public static class OakWoodStairsBlock
    {
        public const string DisplayName = "Oak Wood Stairs";
        public const short BlockId = 53;
        public const double Hardness = 0;
    }
}
