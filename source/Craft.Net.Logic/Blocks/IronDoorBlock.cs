using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(IronDoorBlock.BlockId, IronDoorBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(IronDoorBlock.BlockId, DisplayName = IronDoorBlock.DisplayName, Hardness = IronDoorBlock.Hardness)]
    public static class IronDoorBlock
    {
        public const string DisplayName = "Iron Door";
        public const short BlockId = 71;
		public const double Hardness = 5;
    }
}
