using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [Item(MapItem.ItemId, MapItem.DisplayName)]
    public static class MapItem
    {
        public const short ItemId = 358;
        public const string DisplayName = "Map";
    }
}
