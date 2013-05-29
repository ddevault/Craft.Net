using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(BookshelfBlock.BlockId, BookshelfBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(BookshelfBlock.BlockId, DisplayName = BookshelfBlock.DisplayName, Hardness = BookshelfBlock.Hardness)]
    public static class BookshelfBlock
    {
        public const string DisplayName = "Bookshelf";
        public const short BlockId = 47;
		public const double Hardness = 1.5;
    }
}
