using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(GoldOreBlock.BlockId, GoldOreBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(GoldOreBlock.BlockId, DisplayName = GoldOreBlock.DisplayName, Hardness = GoldOreBlock.Hardness)]
    public static class GoldOreBlock
    {
        public const string DisplayName = "Gold Ore";
        public const short BlockId = 14;
		public const double Hardness = 3;
    }
}
