using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [Item(WrittenBookItem.ItemId, WrittenBookItem.DisplayName)]
    public static class WrittenBookItem
    {
        public const short ItemId = 387;
        public const string DisplayName = "Written Book";
    }
}
