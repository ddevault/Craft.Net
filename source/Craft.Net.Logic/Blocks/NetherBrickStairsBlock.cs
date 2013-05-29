using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(NetherBrickStairsBlock.BlockId, NetherBrickStairsBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(NetherBrickStairsBlock.BlockId, DisplayName = NetherBrickStairsBlock.DisplayName, Hardness = NetherBrickStairsBlock.Hardness)]
    public static class NetherBrickStairsBlock
    {
        public const string DisplayName = "Nether Brick Stairs";
        public const short BlockId = 114;
		public const double Hardness = 0;
    }
}
