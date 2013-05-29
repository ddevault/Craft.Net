using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(DragonEggBlock.BlockId, DragonEggBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(DragonEggBlock.BlockId, DisplayName = DragonEggBlock.DisplayName, Hardness = DragonEggBlock.Hardness)]
    public static class DragonEggBlock
    {
        public const string DisplayName = "Dragon Egg";
        public const short BlockId = 122;
		public const double Hardness = 3;
    }
}
