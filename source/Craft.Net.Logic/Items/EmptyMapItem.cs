using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(EmptyMapItem.ItemId, EmptyMapItem.DisplayName)]
    public static class EmptyMapItem
    {
        public const short ItemId = 395;
        public const string DisplayName = "Empty Map";
    }
}
