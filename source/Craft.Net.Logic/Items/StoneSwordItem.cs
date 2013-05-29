using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(StoneSwordItem.ItemId, StoneSwordItem.DisplayName)]
    public static class StoneSwordItem
    {
        public const short ItemId = 272;
        public const string DisplayName = "Stone Sword";
    }
}
