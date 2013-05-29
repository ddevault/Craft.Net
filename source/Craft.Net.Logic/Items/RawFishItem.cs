using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(RawFishItem.ItemId, RawFishItem.DisplayName)]
    public static class RawFishItem
    {
        public const short ItemId = 349;
        public const string DisplayName = "Raw Fish";
    }
}
