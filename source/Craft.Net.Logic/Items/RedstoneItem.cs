using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [Item(RedstoneItem.ItemId, RedstoneItem.DisplayName)]
    public static class RedstoneItem
    {
        public const short ItemId = 331;
        public const string DisplayName = "Redstone";
    }
}
