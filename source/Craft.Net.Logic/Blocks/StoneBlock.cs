using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(StoneBlock.BlockId, StoneBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(StoneBlock.BlockId, DisplayName = StoneBlock.DisplayName, Hardness = StoneBlock.Hardness)]
    public static class StoneBlock
    {
        public const string DisplayName = "Stone";
        public const short BlockId = 1;
		public const double Hardness = 1.5;
    }
}
