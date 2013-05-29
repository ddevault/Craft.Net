using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(BrickItem.ItemId, BrickItem.DisplayName)]
    public static class BrickItem
    {
        public const short ItemId = 336;
        public const string DisplayName = "Brick";
    }
}
