using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(IronOreBlock.BlockId, IronOreBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(IronOreBlock.BlockId, DisplayName = IronOreBlock.DisplayName, Hardness = IronOreBlock.Hardness)]
    public static class IronOreBlock
    {
        public const string DisplayName = "Iron Ore";
        public const short BlockId = 15;
        public const double Hardness = 3;
    }
}
