using Craft.Net.Common;
using Craft.Net.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Logic
{
    public class ItemEntity : ObjectEntity, IAABBEntity
    {
        public static double PickupRange = 2;

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

        public override bool SendMetadataToClients
        {
            get
            {
                return true;
            }
        }

        public BoundingBox BoundingBox
        {
            get { return new BoundingBox(Position, Position + Size); }
        }

        public void TerrainCollision(PhysicsEngine engine, Vector3 collisionPoint, Vector3 collisionDirection)
        {
        }

        public float AccelerationDueToGravity
        {
            get { return 0.08f; }
        }

        public float Drag
        {
            get { return 0.98f; }
        }

        public bool BeginUpdate()
        {
            EnablePropertyChange = false;
            return true;
        }

        public void EndUpdate(Vector3 newPosition)
        {
            EnablePropertyChange = true;
            Position = newPosition;
        }

        public override void Update(Entity[] nearbyEntities)
        {
            if ((DateTime.Now - SpawnTime).TotalSeconds > 1)
            {
                var player = nearbyEntities.FirstOrDefault(e => e is PlayerEntity && (e as PlayerEntity).Health != 0
                    && e.Position.DistanceTo(Position) <= PickupRange);
                if (player != null)
                    (player as PlayerEntity).OnPickUpItem(this);
                var item = nearbyEntities.FirstOrDefault(e => e is ItemEntity && (DateTime.Now - (e as ItemEntity).SpawnTime).TotalSeconds > 1
                    && (e as ItemEntity).Item.Id == Item.Id && (e as ItemEntity).Item.Metadata == Item.Metadata
                    && (e as ItemEntity).Item.Nbt == Item.Nbt
                    && e.Position.DistanceTo(Position) < PickupRange);
                if (item != null)
                {
                    // Merge
                    item.OnDespawn();
                    var newItem = Item;
                    newItem.Count += (item as ItemEntity).Item.Count;
                    Item = newItem;
                    OnPropertyChanged("Metadata");
                }
            }
        }
    }
}
