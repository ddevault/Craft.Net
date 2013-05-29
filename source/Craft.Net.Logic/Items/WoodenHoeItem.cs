using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(WoodenHoeItem.ItemId, WoodenHoeItem.DisplayName)]
    public static class WoodenHoeItem
    {
        public const short ItemId = 290;
        public const string DisplayName = "Wooden Hoe";
    }
}
