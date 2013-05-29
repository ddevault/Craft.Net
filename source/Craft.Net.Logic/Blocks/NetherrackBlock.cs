using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(NetherrackBlock.BlockId, NetherrackBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(NetherrackBlock.BlockId, DisplayName = NetherrackBlock.DisplayName, Hardness = NetherrackBlock.Hardness)]
    public static class NetherrackBlock
    {
        public const string DisplayName = "Netherrack";
        public const short BlockId = 87;
		public const double Hardness = 0.4;
    }
}
