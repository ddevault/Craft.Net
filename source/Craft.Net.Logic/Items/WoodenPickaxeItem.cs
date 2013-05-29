using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(WoodenPickaxeItem.ItemId, WoodenPickaxeItem.DisplayName)]
    public static class WoodenPickaxeItem
    {
        public const short ItemId = 270;
        public const string DisplayName = "Wooden Pickaxe";
    }
}
