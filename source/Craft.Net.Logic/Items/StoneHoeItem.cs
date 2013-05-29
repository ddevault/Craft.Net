using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(StoneHoeItem.ItemId, StoneHoeItem.DisplayName)]
    public static class StoneHoeItem
    {
        public const short ItemId = 291;
        public const string DisplayName = "Stone Hoe";
    }
}
