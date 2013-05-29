using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(CauldronItem.ItemId, CauldronItem.DisplayName)]
    public static class CauldronItem
    {
        public const short ItemId = 380;
        public const string DisplayName = "Cauldron";
    }
}
