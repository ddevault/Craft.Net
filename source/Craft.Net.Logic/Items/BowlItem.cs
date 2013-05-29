using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(BowlItem.ItemId, BowlItem.DisplayName)]
    public static class BowlItem
    {
        public const short ItemId = 281;
        public const string DisplayName = "Bowl";
    }
}
