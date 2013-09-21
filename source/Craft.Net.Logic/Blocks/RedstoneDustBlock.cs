using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(RedstoneDustBlock.BlockId, RedstoneDustBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(RedstoneDustBlock.BlockId, DisplayName = RedstoneDustBlock.DisplayName, Hardness = RedstoneDustBlock.Hardness)]
    public static class RedstoneDustBlock
    {
        public const string DisplayName = "Redstone Dust";
        public const short BlockId = 55;
        public const double Hardness = 0;
    }
}
