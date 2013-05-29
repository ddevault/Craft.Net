using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [Item(PotatoItem.ItemId, PotatoItem.DisplayName)]
    public static class PotatoItem
    {
        public const short ItemId = 392;
        public const string DisplayName = "Potato";
    }
}
