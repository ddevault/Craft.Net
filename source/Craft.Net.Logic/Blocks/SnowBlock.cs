using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(SnowBlock.BlockId, SnowBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(SnowBlock.BlockId, DisplayName = SnowBlock.DisplayName, Hardness = SnowBlock.Hardness)]
    public static class SnowBlock
    {
        public const string DisplayName = "Snow";
        public const short BlockId = 78;
		public const double Hardness = 0.1;
    }
}
