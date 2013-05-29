using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(StoneBricksBlock.BlockId, StoneBricksBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(StoneBricksBlock.BlockId, DisplayName = StoneBricksBlock.DisplayName, Hardness = StoneBricksBlock.Hardness)]
    public static class StoneBricksBlock
    {
        public const string DisplayName = "Stone Bricks";
        public const short BlockId = 98;
		public const double Hardness = 1.5;
    }
}
