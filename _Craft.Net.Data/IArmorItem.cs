using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data
{
    public enum ArmorSlot
    {
        Headgear = 0,
        Chestplate = 1,
        Pants = 2,
        Footwear = 3
    }

    public interface IArmorItem
    {
        ArmorSlot ArmorSlot { get; }
        int ArmorBonus { get; }
    }
}
