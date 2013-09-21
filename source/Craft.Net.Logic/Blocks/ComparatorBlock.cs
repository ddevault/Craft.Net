using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(ComparatorBlock.BlockId, ComparatorBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(ComparatorBlock.BlockId, DisplayName = ComparatorBlock.DisplayName, Hardness = ComparatorBlock.Hardness)]
    public static class ComparatorBlock
    {
        public const string DisplayName = "Comparator";
        public const short BlockId = 149;
        public const double Hardness = 0;
    }
}
