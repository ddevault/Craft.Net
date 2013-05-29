using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(RedstoneRepeaterItem.ItemId, RedstoneRepeaterItem.DisplayName)]
    public static class RedstoneRepeaterItem
    {
        public const short ItemId = 356;
        public const string DisplayName = "Redstone Repeater";
    }
}
