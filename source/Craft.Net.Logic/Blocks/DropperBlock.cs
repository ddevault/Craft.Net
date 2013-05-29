using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(DropperBlock.BlockId, DropperBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(DropperBlock.BlockId, DisplayName = DropperBlock.DisplayName, Hardness = DropperBlock.Hardness)]
    public static class DropperBlock
    {
        public const string DisplayName = "Dropper";
        public const short BlockId = 158;
		public const double Hardness = 3.5;
    }
}
