using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(FurnaceBlock.BlockId, FurnaceBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(FurnaceBlock.BlockId, DisplayName = FurnaceBlock.DisplayName, Hardness = FurnaceBlock.Hardness)]
    public static class FurnaceBlock
    {
        public const string DisplayName = "Furnace";
        public const short BlockId = 61;
		public const double Hardness = 3.5;
    }
}
