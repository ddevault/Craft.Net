using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(AnvilBlock.BlockId, AnvilBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(AnvilBlock.BlockId, DisplayName = AnvilBlock.DisplayName, Hardness = AnvilBlock.Hardness)]
    public static class AnvilBlock
    {
        public const string DisplayName = "Anvil";
        public const short BlockId = 145;
		public const double Hardness = 5;
    }
}
