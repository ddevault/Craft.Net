using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;
using Craft.Net.Common;

namespace Craft.Net.Logic.Blocks
{
    [Item(GrassBlock.BlockId, GrassBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(GrassBlock.BlockId, GrassBlock.DisplayName, "Initialize", Hardness = GrassBlock.Hardness)]
    public static class GrassBlock
    {
        public const string DisplayName = "Grass Block";
        public const short BlockId = 2;
		public const double Hardness = 0.6;

		public static BlockLogicDescriptor Initialize(BlockLogicDescriptor descriptor)
		{
			descriptor.GetDrop = (b, w, c) => new[] { new ItemStack(DirtBlock.BlockId) };
			return descriptor;
		}
    }
}
