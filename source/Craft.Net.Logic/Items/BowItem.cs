using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(BowItem.ItemId, BowItem.DisplayName)]
    public static class BowItem
    {
        public const short ItemId = 261;
        public const string DisplayName = "Bow";
    }
}
