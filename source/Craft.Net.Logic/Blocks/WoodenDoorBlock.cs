using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(WoodenDoorBlock.BlockId, WoodenDoorBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(WoodenDoorBlock.BlockId, DisplayName = WoodenDoorBlock.DisplayName, Hardness = WoodenDoorBlock.Hardness)]
    public static class WoodenDoorBlock
    {
        public const string DisplayName = "Wooden Door";
        public const short BlockId = 64;
        public const double Hardness = 3;
    }
}
