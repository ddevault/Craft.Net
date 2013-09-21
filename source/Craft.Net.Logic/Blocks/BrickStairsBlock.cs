using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(BrickStairsBlock.BlockId, BrickStairsBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(BrickStairsBlock.BlockId, DisplayName = BrickStairsBlock.DisplayName, Hardness = BrickStairsBlock.Hardness)]
    public static class BrickStairsBlock
    {
        public const string DisplayName = "Brick Stairs";
        public const short BlockId = 108;
        public const double Hardness = 0;
    }
}
