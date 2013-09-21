using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(LadderBlock.BlockId, LadderBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(LadderBlock.BlockId, DisplayName = LadderBlock.DisplayName, Hardness = LadderBlock.Hardness)]
    public static class LadderBlock
    {
        public const string DisplayName = "Ladder";
        public const short BlockId = 65;
        public const double Hardness = 0.4;
    }
}
