using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(DirtBlock.BlockId, DirtBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(DirtBlock.BlockId, DisplayName = DirtBlock.DisplayName, Hardness = DirtBlock.Hardness)]
    public static class DirtBlock
    {
        public const string DisplayName = "Dirt";
        public const short BlockId = 3;
        public const double Hardness = 0.5;
    }
}
