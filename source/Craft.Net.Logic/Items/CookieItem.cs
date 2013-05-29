using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(CookieItem.ItemId, CookieItem.DisplayName)]
    public static class CookieItem
    {
        public const short ItemId = 357;
        public const string DisplayName = "Cookie";
    }
}
