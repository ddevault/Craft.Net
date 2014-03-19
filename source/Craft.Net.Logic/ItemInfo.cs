using System;

namespace Craft.Net.Logic
{
    public class ItemInfo
    {
        public ItemInfo(short itemId, short metadata)
        {
            ItemId = itemId;
            Metadata = metadata;
        }

        public short ItemId;
        public short Metadata;
    }
}