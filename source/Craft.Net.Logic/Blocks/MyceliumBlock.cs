using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(MyceliumBlock.BlockId, MyceliumBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(MyceliumBlock.BlockId, DisplayName = MyceliumBlock.DisplayName, Hardness = MyceliumBlock.Hardness)]
    public static class MyceliumBlock
    {
        public const string DisplayName = "Mycelium";
        public const short BlockId = 110;
		public const double Hardness = 0.6;
    }
}
