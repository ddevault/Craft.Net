using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(SandstoneBlock.BlockId, SandstoneBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(SandstoneBlock.BlockId, DisplayName = SandstoneBlock.DisplayName, Hardness = SandstoneBlock.Hardness)]
    public static class SandstoneBlock
    {
        public const string DisplayName = "Sandstone";
        public const short BlockId = 24;
        public const double Hardness = 0.8;
    }
}
