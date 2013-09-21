using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(RedstoneTorchBlock.BlockId, RedstoneTorchBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(RedstoneTorchBlock.BlockId, DisplayName = RedstoneTorchBlock.DisplayName, Hardness = RedstoneTorchBlock.Hardness)]
    public static class RedstoneTorchBlock
    {
        public const string DisplayName = "Redstone Torch";
        public const short BlockId = 75;
        public const double Hardness = 0;
    }
}
