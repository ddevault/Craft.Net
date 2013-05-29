using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(SteakItem.ItemId, SteakItem.DisplayName)]
    public static class SteakItem
    {
        public const short ItemId = 364;
        public const string DisplayName = "Steak";
    }
}
