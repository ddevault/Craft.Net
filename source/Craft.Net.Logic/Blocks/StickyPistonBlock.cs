using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(StickyPistonBlock.BlockId, StickyPistonBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(StickyPistonBlock.BlockId, DisplayName = StickyPistonBlock.DisplayName, Hardness = StickyPistonBlock.Hardness)]
    public static class StickyPistonBlock
    {
        public const string DisplayName = "Sticky Piston";
        public const short BlockId = 29;
		public const double Hardness = 0;
    }
}
