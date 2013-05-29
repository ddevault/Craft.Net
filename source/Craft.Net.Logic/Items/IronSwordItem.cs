using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(IronSwordItem.ItemId, IronSwordItem.DisplayName)]
    public static class IronSwordItem
    {
        public const short ItemId = 267;
        public const string DisplayName = "Iron Sword";
    }
}
