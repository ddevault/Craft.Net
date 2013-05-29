using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(ChestBlock.BlockId, ChestBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(ChestBlock.BlockId, DisplayName = ChestBlock.DisplayName, Hardness = ChestBlock.Hardness)]
    public static class ChestBlock
    {
        public const string DisplayName = "Chest";
        public const short BlockId = 54;
		public const double Hardness = 2.5;
    }
}
