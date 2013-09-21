using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(ClayBlock.BlockId, ClayBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(ClayBlock.BlockId, DisplayName = ClayBlock.DisplayName, Hardness = ClayBlock.Hardness)]
    public static class ClayBlock
    {
        public const string DisplayName = "Clay";
        public const short BlockId = 82;
        public const double Hardness = 0.6;
    }
}
