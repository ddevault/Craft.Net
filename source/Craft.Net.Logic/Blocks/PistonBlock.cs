using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(PistonBlock.BlockId, PistonBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(PistonBlock.BlockId, DisplayName = PistonBlock.DisplayName, Hardness = PistonBlock.Hardness)]
    public static class PistonBlock
    {
        public const string DisplayName = "Piston";
        public const short BlockId = 33;
		public const double Hardness = 0;
    }
}
