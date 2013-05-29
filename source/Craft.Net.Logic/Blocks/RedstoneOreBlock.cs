using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(RedstoneOreBlock.BlockId, RedstoneOreBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(RedstoneOreBlock.BlockId, DisplayName = RedstoneOreBlock.DisplayName, Hardness = RedstoneOreBlock.Hardness)]
    public static class RedstoneOreBlock
    {
        public const string DisplayName = "Redstone Ore";
        public const short BlockId = 73;
		public const double Hardness = 3;
    }
}
