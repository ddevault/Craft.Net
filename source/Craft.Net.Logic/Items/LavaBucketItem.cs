using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(LavaBucketItem.ItemId, LavaBucketItem.DisplayName)]
    public static class LavaBucketItem
    {
        public const short ItemId = 327;
        public const string DisplayName = "Lava Bucket";
    }
}
