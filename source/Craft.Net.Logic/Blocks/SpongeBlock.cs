using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(SpongeBlock.BlockId, SpongeBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(SpongeBlock.BlockId, DisplayName = SpongeBlock.DisplayName, Hardness = SpongeBlock.Hardness)]
    public static class SpongeBlock
    {
        public const string DisplayName = "Sponge";
        public const short BlockId = 19;
		public const double Hardness = 0.6;
    }
}
