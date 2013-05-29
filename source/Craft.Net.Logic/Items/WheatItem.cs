using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [Item(WheatItem.ItemId, WheatItem.DisplayName)]
    public static class WheatItem
    {
        public const short ItemId = 296;
        public const string DisplayName = "Wheat";
    }
}
