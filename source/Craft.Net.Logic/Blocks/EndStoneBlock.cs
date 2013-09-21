using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(EndStoneBlock.BlockId, EndStoneBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(EndStoneBlock.BlockId, DisplayName = EndStoneBlock.DisplayName, Hardness = EndStoneBlock.Hardness)]
    public static class EndStoneBlock
    {
        public const string DisplayName = "End Stone";
        public const short BlockId = 121;
        public const double Hardness = 3;
    }
}
