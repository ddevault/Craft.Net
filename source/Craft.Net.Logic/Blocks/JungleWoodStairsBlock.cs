using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(JungleWoodStairsBlock.BlockId, JungleWoodStairsBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(JungleWoodStairsBlock.BlockId, DisplayName = JungleWoodStairsBlock.DisplayName, Hardness = JungleWoodStairsBlock.Hardness)]
    public static class JungleWoodStairsBlock
    {
        public const string DisplayName = "Jungle Wood Stairs";
        public const short BlockId = 136;
        public const double Hardness = 0;
    }
}
