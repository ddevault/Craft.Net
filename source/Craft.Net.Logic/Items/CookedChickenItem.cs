using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [Item(CookedChickenItem.ItemId, CookedChickenItem.DisplayName)]
    public static class CookedChickenItem
    {
        public const short ItemId = 366;
        public const string DisplayName = "Cooked Chicken";
    }
}
