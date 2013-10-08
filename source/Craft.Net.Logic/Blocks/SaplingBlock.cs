using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(SaplingBlock.BlockId, SaplingBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(SaplingBlock.BlockId, DisplayName = SaplingBlock.DisplayName, Hardness = SaplingBlock.Hardness)]
    public static class SaplingBlock
    {
        public const string DisplayName = "Sapling";
        public const short BlockId = 6;
        public const double Hardness = 0;
    }
}
