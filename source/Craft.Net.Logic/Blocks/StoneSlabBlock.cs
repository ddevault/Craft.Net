using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(StoneSlabBlock.BlockId, StoneSlabBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(StoneSlabBlock.BlockId, DisplayName = StoneSlabBlock.DisplayName, Hardness = StoneSlabBlock.Hardness)]
    public static class StoneSlabBlock
    {
        public const string DisplayName = "StoneSlab";
        public const short BlockId = 43;
        public const double Hardness = 2;
    }
}
