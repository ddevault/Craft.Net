using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(GoldNuggetItem.ItemId, GoldNuggetItem.DisplayName)]
    public static class GoldNuggetItem
    {
        public const short ItemId = 371;
        public const string DisplayName = "Gold Nugget";
    }
}
