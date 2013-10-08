using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(LavaBlock.BlockId, LavaBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(LavaBlock.BlockId, DisplayName = LavaBlock.DisplayName, Hardness = LavaBlock.Hardness)]
    public static class LavaBlock
    {
        public const string DisplayName = "Lava";
        public const short BlockId = 10;
        public const double Hardness = 0;
    }
}
