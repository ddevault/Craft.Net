using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(ClayItem.ItemId, ClayItem.DisplayName)]
    public static class ClayItem
    {
        public const short ItemId = 337;
        public const string DisplayName = "Clay";
    }
}
