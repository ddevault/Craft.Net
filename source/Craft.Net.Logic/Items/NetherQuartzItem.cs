using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(NetherQuartzItem.ItemId, NetherQuartzItem.DisplayName)]
    public static class NetherQuartzItem
    {
        public const short ItemId = 406;
        public const string DisplayName = "Nether Quartz";
    }
}
