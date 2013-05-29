using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [Item(StringItem.ItemId, StringItem.DisplayName)]
    public static class StringItem
    {
        public const short ItemId = 287;
        public const string DisplayName = "String";
    }
}
