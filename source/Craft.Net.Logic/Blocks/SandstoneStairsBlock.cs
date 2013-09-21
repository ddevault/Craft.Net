using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(SandstoneStairsBlock.BlockId, SandstoneStairsBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(SandstoneStairsBlock.BlockId, DisplayName = SandstoneStairsBlock.DisplayName, Hardness = SandstoneStairsBlock.Hardness)]
    public static class SandstoneStairsBlock
    {
        public const string DisplayName = "Sandstone Stairs";
        public const short BlockId = 128;
        public const double Hardness = 0;
    }
}
