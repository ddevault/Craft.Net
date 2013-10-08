using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(RailBlock.BlockId, RailBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(RailBlock.BlockId, DisplayName = RailBlock.DisplayName, Hardness = RailBlock.Hardness)]
    public static class RailBlock
    {
        public const string DisplayName = "Rail";
        public const short BlockId = 66;
        public const double Hardness = 0.7;
    }
}
