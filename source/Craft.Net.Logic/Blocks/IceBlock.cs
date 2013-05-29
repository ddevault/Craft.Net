using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(IceBlock.BlockId, IceBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(IceBlock.BlockId, DisplayName = IceBlock.DisplayName, Hardness = IceBlock.Hardness)]
    public static class IceBlock
    {
        public const string DisplayName = "Ice";
        public const short BlockId = 79;
		public const double Hardness = 0.5;
    }
}
