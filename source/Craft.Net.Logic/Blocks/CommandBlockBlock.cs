using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(CommandBlockBlock.BlockId, CommandBlockBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(CommandBlockBlock.BlockId, DisplayName = CommandBlockBlock.DisplayName, Hardness = CommandBlockBlock.Hardness)]
    public static class CommandBlockBlock
    {
        public const string DisplayName = "Command Block";
        public const short BlockId = 137;
		public const double Hardness = 0;
    }
}
