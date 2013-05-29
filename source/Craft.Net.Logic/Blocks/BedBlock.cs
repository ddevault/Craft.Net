using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(BedBlock.BlockId, BedBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(BedBlock.BlockId, DisplayName = BedBlock.DisplayName, Hardness = BedBlock.Hardness)]
    public static class BedBlock
    {
        public const string DisplayName = "Bed";
        public const short BlockId = 26;
		public const double Hardness = 0.2;
    }
}
