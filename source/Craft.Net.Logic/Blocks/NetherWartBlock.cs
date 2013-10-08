using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(NetherWartBlock.BlockId, NetherWartBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(NetherWartBlock.BlockId, DisplayName = NetherWartBlock.DisplayName, Hardness = NetherWartBlock.Hardness)]
    public static class NetherWartBlock
    {
        public const string DisplayName = "Nether Wart";
        public const short BlockId = 115;
        public const double Hardness = 0;
    }
}
