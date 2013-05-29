using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(IronHoeItem.ItemId, IronHoeItem.DisplayName)]
    public static class IronHoeItem
    {
        public const short ItemId = 292;
        public const string DisplayName = "Iron Hoe";
    }
}
