using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(CoalOreBlock.BlockId, CoalOreBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(CoalOreBlock.BlockId, DisplayName = CoalOreBlock.DisplayName, Hardness = CoalOreBlock.Hardness)]
    public static class CoalOreBlock
    {
        public const string DisplayName = "Coal Ore";
        public const short BlockId = 16;
        public const double Hardness = 3;
    }
}
