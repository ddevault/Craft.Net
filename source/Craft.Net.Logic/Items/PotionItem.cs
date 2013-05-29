using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [Item(PotionItem.ItemId, PotionItem.DisplayName)]
    public static class PotionItem
    {
        public const short ItemId = 373;
        public const string DisplayName = "Potion";
    }
}
