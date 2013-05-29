using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [Item(CookedFishItem.ItemId, CookedFishItem.DisplayName)]
    public static class CookedFishItem
    {
        public const short ItemId = 350;
        public const string DisplayName = "Cooked Fish";
    }
}
