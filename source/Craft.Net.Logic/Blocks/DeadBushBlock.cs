using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(DeadBushBlock.BlockId, DeadBushBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(DeadBushBlock.BlockId, DisplayName = DeadBushBlock.DisplayName, Hardness = DeadBushBlock.Hardness)]
    public static class DeadBushBlock
    {
        public const string DisplayName = "Dead Bush";
        public const short BlockId = 32;
		public const double Hardness = 0;
    }
}
