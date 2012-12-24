using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Events;
using Craft.Net.Metadata;

namespace Craft.Net.Data.Entities
{
    public class ItemEntity : ObjectEntity
    {
        private DateTime SpawnTime { get; set; }

        public ItemEntity(Vector3 position, ItemStack item)
        {
            Position = position;
            Item = item;
            SpawnTime = DateTime.Now;
        }

        public override Size Size
        {
            get { return new Size(0.25, 0.25, 0.25); }
        }

        public ItemStack Item { get; set; }

        public override float AccelerationDueToGravity
        {
            get { return 0.04f; }
        }

        public override float Drag
        {
            get { return 0.98f; }
        }

        public override void PhysicsUpdate(World world)
        {
            base.PhysicsUpdate(world);
            if ((DateTime.Now - SpawnTime).TotalSeconds > 0.5 && world.Level.Time % 10 == 0)
            {
                var player = (PlayerEntity)world.Entities.FirstOrDefault(e => e is PlayerEntity &&
                    e.Position.DistanceTo(Position) <= 1);
                if (player != null)
                    player.OnPickUpItem(new EntityEventArgs(this));
            }
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
                metadata[10] = new MetadataSlot(10, Item);
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
