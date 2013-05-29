using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(LeatherItem.ItemId, LeatherItem.DisplayName)]
    public static class LeatherItem
    {
        public const short ItemId = 334;
        public const string DisplayName = "Leather";
    }
}
