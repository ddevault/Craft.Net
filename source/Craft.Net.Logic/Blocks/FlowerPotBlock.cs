using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(FlowerPotBlock.BlockId, FlowerPotBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(FlowerPotBlock.BlockId, DisplayName = FlowerPotBlock.DisplayName, Hardness = FlowerPotBlock.Hardness)]
    public static class FlowerPotBlock
    {
        public const string DisplayName = "FlowerPot";
        public const short BlockId = 140;
		public const double Hardness = 0;
    }
}
