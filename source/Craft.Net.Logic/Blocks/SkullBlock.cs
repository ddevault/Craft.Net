using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(SkullBlock.BlockId, SkullBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(SkullBlock.BlockId, DisplayName = SkullBlock.DisplayName, Hardness = SkullBlock.Hardness)]
    public static class SkullBlock
    {
        public const string DisplayName = "Skull";
        public const short BlockId = 144;
		public const double Hardness = 1;
    }
}
