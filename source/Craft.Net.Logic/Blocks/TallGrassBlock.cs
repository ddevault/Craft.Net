using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;
using Craft.Net.Common;

namespace Craft.Net.Logic.Blocks
{
    [Item(TallGrassBlock.BlockId, TallGrassBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(TallGrassBlock.BlockId, TallGrassBlock.DisplayName, "Initialize", Hardness = TallGrassBlock.Hardness)]
    public static class TallGrassBlock
    {
        public const string DisplayName = "Grass";
        public const short BlockId = 31;
		public const double Hardness = 0;

		public static BlockLogicDescriptor Initialize(BlockLogicDescriptor descriptor)
		{
			//descriptor.GetDrop = (b, w, c) => new[] { new ItemStack(DirtBlock.BlockId) };
			return descriptor;
		}
    }
}
