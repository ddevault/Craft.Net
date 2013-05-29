using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(FenceGateBlock.BlockId, FenceGateBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(FenceGateBlock.BlockId, DisplayName = FenceGateBlock.DisplayName, Hardness = FenceGateBlock.Hardness)]
    public static class FenceGateBlock
    {
        public const string DisplayName = "Fence Gate";
        public const short BlockId = 107;
		public const double Hardness = 2;
    }
}
