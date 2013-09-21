using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(DiamondOreBlock.BlockId, DiamondOreBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(DiamondOreBlock.BlockId, DisplayName = DiamondOreBlock.DisplayName, Hardness = DiamondOreBlock.Hardness)]
    public static class DiamondOreBlock
    {
        public const string DisplayName = "Diamond Ore";
        public const short BlockId = 56;
        public const double Hardness = 3;
    }
}
