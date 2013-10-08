using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(NetherBrickFenceBlock.BlockId, NetherBrickFenceBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(NetherBrickFenceBlock.BlockId, DisplayName = NetherBrickFenceBlock.DisplayName, Hardness = NetherBrickFenceBlock.Hardness)]
    public static class NetherBrickFenceBlock
    {
        public const string DisplayName = "Nether Brick Fence";
        public const short BlockId = 113;
        public const double Hardness = 2;
    }
}
