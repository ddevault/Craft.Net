using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [Item(BedItem.ItemId, BedItem.DisplayName)]
    public static class BedItem
    {
        public const short ItemId = 355;
        public const string DisplayName = "Bed";
    }
}
