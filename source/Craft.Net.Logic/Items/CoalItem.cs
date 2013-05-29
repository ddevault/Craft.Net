using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(CoalItem.ItemId, CoalItem.DisplayName)]
    public static class CoalItem
    {
        public const short ItemId = 263;
        public const string DisplayName = "Coal";
    }
}
