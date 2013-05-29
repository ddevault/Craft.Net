using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(NoteBlockBlock.BlockId, NoteBlockBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(NoteBlockBlock.BlockId, DisplayName = NoteBlockBlock.DisplayName, Hardness = NoteBlockBlock.Hardness)]
    public static class NoteBlockBlock
    {
        public const string DisplayName = "Note Block";
        public const short BlockId = 25;
		public const double Hardness = 0.8;
    }
}
