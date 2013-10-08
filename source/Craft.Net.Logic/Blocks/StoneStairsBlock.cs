using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(StoneStairsBlock.BlockId, StoneStairsBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(StoneStairsBlock.BlockId, DisplayName = StoneStairsBlock.DisplayName, Hardness = StoneStairsBlock.Hardness)]
    public static class StoneStairsBlock
    {
        public const string DisplayName = "Stone Stairs";
        public const short BlockId = 67;
        public const double Hardness = 0;
    }
}
