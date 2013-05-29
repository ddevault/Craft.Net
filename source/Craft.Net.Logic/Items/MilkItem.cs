using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(MilkItem.ItemId, MilkItem.DisplayName)]
    public static class MilkItem
    {
        public const short ItemId = 335;
        public const string DisplayName = "Milk";
    }
}
