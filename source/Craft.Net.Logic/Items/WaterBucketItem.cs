using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(WaterBucketItem.ItemId, WaterBucketItem.DisplayName)]
    public static class WaterBucketItem
    {
        public const short ItemId = 326;
        public const string DisplayName = "Water Bucket";
    }
}
