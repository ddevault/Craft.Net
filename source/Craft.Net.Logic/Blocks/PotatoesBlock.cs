using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(PotatoesBlock.BlockId, PotatoesBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(PotatoesBlock.BlockId, DisplayName = PotatoesBlock.DisplayName, Hardness = PotatoesBlock.Hardness)]
    public static class PotatoesBlock
    {
        public const string DisplayName = "Potatoes";
        public const short BlockId = 142;
        public const double Hardness = 0;
    }
}
