using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(MossStoneBlock.BlockId, MossStoneBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(MossStoneBlock.BlockId, DisplayName = MossStoneBlock.DisplayName, Hardness = MossStoneBlock.Hardness)]
    public static class MossStoneBlock
    {
        public const string DisplayName = "Moss Stone";
        public const short BlockId = 48;
		public const double Hardness = 2;
    }
}
