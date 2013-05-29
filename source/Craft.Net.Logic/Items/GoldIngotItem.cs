using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [Item(GoldIngotItem.ItemId, GoldIngotItem.DisplayName)]
    public static class GoldIngotItem
    {
        public const short ItemId = 266;
        public const string DisplayName = "Gold Ingot";
    }
}
