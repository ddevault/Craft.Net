using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(FireChargeItem.ItemId, FireChargeItem.DisplayName)]
    public static class FireChargeItem
    {
        public const short ItemId = 385;
        public const string DisplayName = "Fire Charge";
    }
}
