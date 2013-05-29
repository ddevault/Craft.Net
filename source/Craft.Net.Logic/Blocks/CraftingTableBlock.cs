using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(CraftingTableBlock.BlockId, CraftingTableBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(CraftingTableBlock.BlockId, DisplayName = CraftingTableBlock.DisplayName, Hardness = CraftingTableBlock.Hardness)]
    public static class CraftingTableBlock
    {
        public const string DisplayName = "Crafting Table";
        public const short BlockId = 58;
		public const double Hardness = 2.5;
    }
}
