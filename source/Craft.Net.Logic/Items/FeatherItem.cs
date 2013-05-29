using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(FeatherItem.ItemId, FeatherItem.DisplayName)]
    public static class FeatherItem
    {
        public const short ItemId = 288;
        public const string DisplayName = "Feather";
    }
}
