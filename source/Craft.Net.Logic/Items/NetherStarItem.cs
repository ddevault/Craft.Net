using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(NetherStarItem.ItemId, NetherStarItem.DisplayName)]
    public static class NetherStarItem
    {
        public const short ItemId = 399;
        public const string DisplayName = "Nether Star";
    }
}
