using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(GrassBlock.BlockId, GrassBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(GrassBlock.BlockId, DisplayName = GrassBlock.DisplayName, Hardness = GrassBlock.Hardness)]
    public static class GrassBlock
    {
        public const string DisplayName = "Grass";
        public const short BlockId = 31;
		public const double Hardness = 0;
    }
}
