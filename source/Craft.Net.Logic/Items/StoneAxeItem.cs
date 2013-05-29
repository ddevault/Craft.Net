using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(StoneAxeItem.ItemId, StoneAxeItem.DisplayName)]
    public static class StoneAxeItem
    {
        public const short ItemId = 275;
        public const string DisplayName = "Stone Axe";
    }
}
