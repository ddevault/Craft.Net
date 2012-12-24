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
            return (Item)slot.Id;
        }
    }
}
