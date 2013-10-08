using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(LilyPadBlock.BlockId, LilyPadBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(LilyPadBlock.BlockId, DisplayName = LilyPadBlock.DisplayName, Hardness = LilyPadBlock.Hardness)]
    public static class LilyPadBlock
    {
        public const string DisplayName = "Lily Pad";
        public const short BlockId = 111;
        public const double Hardness = 0;
    }
}
