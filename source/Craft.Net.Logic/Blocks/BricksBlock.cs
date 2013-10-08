using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(BricksBlock.BlockId, BricksBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(BricksBlock.BlockId, DisplayName = BricksBlock.DisplayName, Hardness = BricksBlock.Hardness)]
    public static class BricksBlock
    {
        public const string DisplayName = "Bricks";
        public const short BlockId = 45;
        public const double Hardness = 2;
    }
}
