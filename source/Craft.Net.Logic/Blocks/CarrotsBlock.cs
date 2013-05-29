using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(CarrotsBlock.BlockId, CarrotsBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(CarrotsBlock.BlockId, DisplayName = CarrotsBlock.DisplayName, Hardness = CarrotsBlock.Hardness)]
    public static class CarrotsBlock
    {
        public const string DisplayName = "Carrots";
        public const short BlockId = 141;
		public const double Hardness = 0;
    }
}
