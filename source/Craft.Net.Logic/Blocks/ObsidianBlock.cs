using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(ObsidianBlock.BlockId, ObsidianBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(ObsidianBlock.BlockId, DisplayName = ObsidianBlock.DisplayName, Hardness = ObsidianBlock.Hardness)]
    public static class ObsidianBlock
    {
        public const string DisplayName = "Obsidian";
        public const short BlockId = 49;
		public const double Hardness = 50;
    }
}
