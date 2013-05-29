using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(JackoLanternBlock.BlockId, JackoLanternBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(JackoLanternBlock.BlockId, DisplayName = JackoLanternBlock.DisplayName, Hardness = JackoLanternBlock.Hardness)]
    public static class JackoLanternBlock
    {
        public const string DisplayName = "Jack 'o' Lantern";
        public const short BlockId = 91;
		public const double Hardness = 1;
    }
}
