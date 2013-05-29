using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(AppleItem.ItemId, AppleItem.DisplayName)]
    public static class AppleItem
    {
        public const short ItemId = 260;
        public const string DisplayName = "Apple";
    }
}
