using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(FireBlock.BlockId, FireBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(FireBlock.BlockId, DisplayName = FireBlock.DisplayName, Hardness = FireBlock.Hardness)]
    public static class FireBlock
    {
        public const string DisplayName = "Fire";
        public const short BlockId = 51;
		public const double Hardness = 0;
    }
}
