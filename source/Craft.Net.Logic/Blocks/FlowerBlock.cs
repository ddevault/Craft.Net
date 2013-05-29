using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(FlowerBlock.BlockId, FlowerBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(FlowerBlock.BlockId, DisplayName = FlowerBlock.DisplayName, Hardness = FlowerBlock.Hardness)]
    public static class FlowerBlock
    {
        public const string DisplayName = "Flower";
        public const short BlockId = 37;
		public const double Hardness = 0;
    }
}
