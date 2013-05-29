using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [Item(CompassItem.ItemId, CompassItem.DisplayName)]
    public static class CompassItem
    {
        public const short ItemId = 345;
        public const string DisplayName = "Compass";
    }
}
