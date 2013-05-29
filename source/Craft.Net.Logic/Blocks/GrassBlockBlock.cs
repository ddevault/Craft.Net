using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(GrassBlockBlock.BlockId, GrassBlockBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(GrassBlockBlock.BlockId, DisplayName = GrassBlockBlock.DisplayName, Hardness = GrassBlockBlock.Hardness)]
    public static class GrassBlockBlock
    {
        public const string DisplayName = "Grass Block";
        public const short BlockId = 2;
		public const double Hardness = 0.6;
    }
}
