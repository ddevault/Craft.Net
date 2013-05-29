using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(CookedPorkchopItem.ItemId, CookedPorkchopItem.DisplayName)]
    public static class CookedPorkchopItem
    {
        public const short ItemId = 320;
        public const string DisplayName = "Cooked Porkchop";
    }
}
