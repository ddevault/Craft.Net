using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(CocoaBlock.BlockId, CocoaBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(CocoaBlock.BlockId, DisplayName = CocoaBlock.DisplayName, Hardness = CocoaBlock.Hardness)]
    public static class CocoaBlock
    {
        public const string DisplayName = "Cocoa";
        public const short BlockId = 127;
        public const double Hardness = 0.2;
    }
}
