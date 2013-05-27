using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data
{
    public static class Extensions
    {
        public static Item AsItem(this ItemStack slot)
        {
            var item = (Item)slot.Id;
            if (item != null)
                item.Data = slot.Metadata;
            return item;
        }
    }
}
