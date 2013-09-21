using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(TNTBlock.BlockId, TNTBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(TNTBlock.BlockId, DisplayName = TNTBlock.DisplayName, Hardness = TNTBlock.Hardness)]
    public static class TNTBlock
    {
        public const string DisplayName = "TNT";
        public const short BlockId = 46;
        public const double Hardness = 0;
    }
}
