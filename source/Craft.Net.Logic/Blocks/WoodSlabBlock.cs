using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(WoodSlabBlock.BlockId, WoodSlabBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(WoodSlabBlock.BlockId, DisplayName = WoodSlabBlock.DisplayName, Hardness = WoodSlabBlock.Hardness)]
    public static class WoodSlabBlock
    {
        public const string DisplayName = "WoodSlab";
        public const short BlockId = 125;
        public const double Hardness = 2;
    }
}
