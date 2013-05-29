using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(LockedchestBlock.BlockId, LockedchestBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(LockedchestBlock.BlockId, DisplayName = LockedchestBlock.DisplayName, Hardness = LockedchestBlock.Hardness)]
    public static class LockedchestBlock
    {
        public const string DisplayName = "Locked chest";
        public const short BlockId = 95;
		public const double Hardness = 0;
    }
}
