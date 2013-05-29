using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(MusicDiscItem.ItemId, MusicDiscItem.DisplayName)]
    public static class MusicDiscItem
    {
        public const short ItemId = 2267;
        public const string DisplayName = "Music Disc";
    }
}
