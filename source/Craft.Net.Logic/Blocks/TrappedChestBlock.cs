using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(TrappedChestBlock.BlockId, TrappedChestBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(TrappedChestBlock.BlockId, DisplayName = TrappedChestBlock.DisplayName, Hardness = TrappedChestBlock.Hardness)]
    public static class TrappedChestBlock
    {
        public const string DisplayName = "Trapped Chest";
        public const short BlockId = 146;
		public const double Hardness = 2.5;
    }
}
