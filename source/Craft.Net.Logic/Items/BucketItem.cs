using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [MinecraftItem(BucketItem.ItemId, BucketItem.DisplayName)]
    public static class BucketItem
    {
        public const short ItemId = 325;
        public const string DisplayName = "Bucket";
    }
}
