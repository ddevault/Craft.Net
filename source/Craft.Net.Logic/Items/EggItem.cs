using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [Item(EggItem.ItemId, EggItem.DisplayName)]
    public static class EggItem
    {
        public const short ItemId = 344;
        public const string DisplayName = "Egg";
    }
}
