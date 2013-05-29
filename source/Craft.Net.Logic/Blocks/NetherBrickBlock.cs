using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(NetherBrickBlock.BlockId, NetherBrickBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(NetherBrickBlock.BlockId, DisplayName = NetherBrickBlock.DisplayName, Hardness = NetherBrickBlock.Hardness)]
    public static class NetherBrickBlock
    {
        public const string DisplayName = "Nether Brick";
        public const short BlockId = 112;
		public const double Hardness = 2;
    }
}
