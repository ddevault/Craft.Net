using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(BreadItem.ItemId, BreadItem.DisplayName)]
    public static class BreadItem
    {
        public const short ItemId = 297;
        public const string DisplayName = "Bread";
    }
}
