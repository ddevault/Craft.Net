using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(HopperBlock.BlockId, HopperBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(HopperBlock.BlockId, DisplayName = HopperBlock.DisplayName, Hardness = HopperBlock.Hardness)]
    public static class HopperBlock
    {
        public const string DisplayName = "Hopper";
        public const short BlockId = 154;
        public const double Hardness = 3;
    }
}
