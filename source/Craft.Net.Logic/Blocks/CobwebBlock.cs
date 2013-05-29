using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(CobwebBlock.BlockId, CobwebBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(CobwebBlock.BlockId, DisplayName = CobwebBlock.DisplayName, Hardness = CobwebBlock.Hardness)]
    public static class CobwebBlock
    {
        public const string DisplayName = "Cobweb";
        public const short BlockId = 30;
		public const double Hardness = 4;
    }
}
