using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(BrewingStandBlock.BlockId, BrewingStandBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(BrewingStandBlock.BlockId, DisplayName = BrewingStandBlock.DisplayName, Hardness = BrewingStandBlock.Hardness)]
    public static class BrewingStandBlock
    {
        public const string DisplayName = "BrewingStand";
        public const short BlockId = 117;
        public const double Hardness = 0.5;
    }
}
