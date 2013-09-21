using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(TorchBlock.BlockId, TorchBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(TorchBlock.BlockId, DisplayName = TorchBlock.DisplayName, Hardness = TorchBlock.Hardness)]
    public static class TorchBlock
    {
        public const string DisplayName = "Torch";
        public const short BlockId = 50;
        public const double Hardness = 0;
    }
}
