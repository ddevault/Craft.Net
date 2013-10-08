using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(LeverBlock.BlockId, LeverBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(LeverBlock.BlockId, DisplayName = LeverBlock.DisplayName, Hardness = LeverBlock.Hardness)]
    public static class LeverBlock
    {
        public const string DisplayName = "Lever";
        public const short BlockId = 69;
        public const double Hardness = 0.5;
    }
}
