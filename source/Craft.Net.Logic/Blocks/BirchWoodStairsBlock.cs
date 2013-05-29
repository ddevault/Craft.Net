using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(BirchWoodStairsBlock.BlockId, BirchWoodStairsBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(BirchWoodStairsBlock.BlockId, DisplayName = BirchWoodStairsBlock.DisplayName, Hardness = BirchWoodStairsBlock.Hardness)]
    public static class BirchWoodStairsBlock
    {
        public const string DisplayName = "Birch Wood Stairs";
        public const short BlockId = 135;
		public const double Hardness = 0;
    }
}
