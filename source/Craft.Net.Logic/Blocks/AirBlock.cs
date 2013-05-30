using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Logic.Blocks
{
    [Item(AirBlock.BlockId, AirBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(AirBlock.BlockId, DisplayName = AirBlock.DisplayName, Hardness = AirBlock.Hardness)]
    public static class AirBlock
    {
        public const short BlockId = 0;
        public const double Hardness = 0;
        public const string DisplayName = "Air";
    }
}
