using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(TripwireHookBlock.BlockId, TripwireHookBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(TripwireHookBlock.BlockId, DisplayName = TripwireHookBlock.DisplayName, Hardness = TripwireHookBlock.Hardness)]
    public static class TripwireHookBlock
    {
        public const string DisplayName = "Tripwire Hook";
        public const short BlockId = 131;
		public const double Hardness = 0;
    }
}
