using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(RedstoneLampBlock.BlockId, RedstoneLampBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(RedstoneLampBlock.BlockId, DisplayName = RedstoneLampBlock.DisplayName, Hardness = RedstoneLampBlock.Hardness)]
    public static class RedstoneLampBlock
    {
        public const string DisplayName = "Redstone Lamp";
        public const short BlockId = 123;
        public const double Hardness = 0.3;
    }
}
