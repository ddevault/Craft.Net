using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [Item(BookItem.ItemId, BookItem.DisplayName)]
    public static class BookItem
    {
        public const short ItemId = 340;
        public const string DisplayName = "Book";
    }
}
