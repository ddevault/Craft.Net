using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(VinesBlock.BlockId, VinesBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(VinesBlock.BlockId, DisplayName = VinesBlock.DisplayName, Hardness = VinesBlock.Hardness)]
    public static class VinesBlock
    {
        public const string DisplayName = "Vines";
        public const short BlockId = 106;
        public const double Hardness = 0.2;
    }
}
