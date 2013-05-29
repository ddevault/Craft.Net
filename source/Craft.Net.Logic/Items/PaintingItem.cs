using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [Item(PaintingItem.ItemId, PaintingItem.DisplayName)]
    public static class PaintingItem
    {
        public const short ItemId = 321;
        public const string DisplayName = "Painting";
    }
}
