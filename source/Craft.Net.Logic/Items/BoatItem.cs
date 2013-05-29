using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [Item(BoatItem.ItemId, BoatItem.DisplayName)]
    public static class BoatItem
    {
        public const short ItemId = 333;
        public const string DisplayName = "Boat";
    }
}
