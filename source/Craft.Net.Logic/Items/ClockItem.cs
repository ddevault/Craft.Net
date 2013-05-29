using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(ClockItem.ItemId, ClockItem.DisplayName)]
    public static class ClockItem
    {
        public const short ItemId = 347;
        public const string DisplayName = "Clock";
    }
}
