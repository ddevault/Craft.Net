using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(SnowballItem.ItemId, SnowballItem.DisplayName)]
    public static class SnowballItem
    {
        public const short ItemId = 332;
        public const string DisplayName = "Snowball";
    }
}
