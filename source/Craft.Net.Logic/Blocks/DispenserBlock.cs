using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(DispenserBlock.BlockId, DispenserBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(DispenserBlock.BlockId, DisplayName = DispenserBlock.DisplayName, Hardness = DispenserBlock.Hardness)]
    public static class DispenserBlock
    {
        public const string DisplayName = "Dispenser";
        public const short BlockId = 23;
		public const double Hardness = 3.5;
    }
}
