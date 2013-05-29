using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [Item(IronIngotItem.ItemId, IronIngotItem.DisplayName)]
    public static class IronIngotItem
    {
        public const short ItemId = 265;
        public const string DisplayName = "Iron Ingot";
    }
}
