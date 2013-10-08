using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(CauldronBlock.BlockId, CauldronBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(CauldronBlock.BlockId, DisplayName = CauldronBlock.DisplayName, Hardness = CauldronBlock.Hardness)]
    public static class CauldronBlock
    {
        public const string DisplayName = "Cauldron";
        public const short BlockId = 118;
        public const double Hardness = 2;
    }
}
