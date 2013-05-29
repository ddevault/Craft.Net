using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(BrewingStandItem.ItemId, BrewingStandItem.DisplayName)]
    public static class BrewingStandItem
    {
        public const short ItemId = 379;
        public const string DisplayName = "Brewing Stand";
    }
}
