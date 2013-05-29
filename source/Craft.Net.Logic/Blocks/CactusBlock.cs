using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(CactusBlock.BlockId, CactusBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(CactusBlock.BlockId, DisplayName = CactusBlock.DisplayName, Hardness = CactusBlock.Hardness)]
    public static class CactusBlock
    {
        public const string DisplayName = "Cactus";
        public const short BlockId = 81;
		public const double Hardness = 0.4;
    }
}
