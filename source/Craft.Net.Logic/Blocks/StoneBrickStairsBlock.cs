using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(StoneBrickStairsBlock.BlockId, StoneBrickStairsBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(StoneBrickStairsBlock.BlockId, DisplayName = StoneBrickStairsBlock.DisplayName, Hardness = StoneBrickStairsBlock.Hardness)]
    public static class StoneBrickStairsBlock
    {
        public const string DisplayName = "Stone Brick Stairs";
        public const short BlockId = 109;
		public const double Hardness = 0;
    }
}
