using Craft.Net.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Entities
{
    public class ItemEntity : ObjectEntity
    {
        public ItemEntity(Vector3 position, ItemStack item)
        {
            Position = position;
            Item = item;
            SpawnTime = DateTime.Now;
        }

        public ItemStack Item { get; set; }

        private DateTime SpawnTime { get; set; }

        public override Size Size
        {
            get { return new Size(0.25, 0.25, 0.25); }
        }

        public override byte EntityType
        {
            get { return 2; }
        }

        public override int Data
        {
            get { return 1; }
        }

        public override MetadataDictionary Metadata
        {
            get
            {
                var metadata = base.Metadata;
                metadata[10] = Item;
                return metadata;
            }
        }

        public override bool IncludeMetadataOnClient
        {
            get
            {
                return true;
            }
        }
    }
}
