using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(IronDoorItem.ItemId, IronDoorItem.DisplayName)]
    public static class IronDoorItem
    {
        public const short ItemId = 330;
        public const string DisplayName = "Iron Door";
    }
}
