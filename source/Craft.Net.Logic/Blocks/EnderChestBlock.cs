using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(EnderChestBlock.BlockId, EnderChestBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(EnderChestBlock.BlockId, DisplayName = EnderChestBlock.DisplayName, Hardness = EnderChestBlock.Hardness)]
    public static class EnderChestBlock
    {
        public const string DisplayName = "Ender Chest";
        public const short BlockId = 130;
		public const double Hardness = 22.5;
    }
}
