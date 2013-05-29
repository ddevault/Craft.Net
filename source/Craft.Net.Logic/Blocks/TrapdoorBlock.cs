using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(TrapdoorBlock.BlockId, TrapdoorBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(TrapdoorBlock.BlockId, DisplayName = TrapdoorBlock.DisplayName, Hardness = TrapdoorBlock.Hardness)]
    public static class TrapdoorBlock
    {
        public const string DisplayName = "Trapdoor";
        public const short BlockId = 96;
		public const double Hardness = 3;
    }
}
