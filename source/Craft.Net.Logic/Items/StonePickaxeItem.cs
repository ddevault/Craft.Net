using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(StonePickaxeItem.ItemId, StonePickaxeItem.DisplayName)]
    public static class StonePickaxeItem
    {
        public const short ItemId = 274;
        public const string DisplayName = "Stone Pickaxe";
    }
}
