using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(TripwireBlock.BlockId, TripwireBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(TripwireBlock.BlockId, DisplayName = TripwireBlock.DisplayName, Hardness = TripwireBlock.Hardness)]
    public static class TripwireBlock
    {
        public const string DisplayName = "Tripwire";
        public const short BlockId = 132;
        public const double Hardness = 0;
    }
}
