using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(FenceBlock.BlockId, FenceBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(FenceBlock.BlockId, DisplayName = FenceBlock.DisplayName, Hardness = FenceBlock.Hardness)]
    public static class FenceBlock
    {
        public const string DisplayName = "Fence";
        public const short BlockId = 85;
        public const double Hardness = 2;
    }
}
