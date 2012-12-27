using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Metadata;

namespace Craft.Net.Data.Entities
{
    public class ItemFrameEntity : ObjectEntity
    {
        public byte Orientation { get; set; }
        public ItemFrameDirection Direction { get; set; }
        private ItemStack item;
        public ItemStack Item
        {
            get { return item; }
            set
            {
                item = value;
                OnPropertyChanged("Metadata");
            }
        }

        public ItemFrameEntity(ItemStack item, ItemFrameDirection direction, Vector3 position)
        {
            Item = item;
            Position = position;
            Direction = direction;
            Pitch = GetRotation(Direction); // This should be Pitch, but it doesn't work. Not sure why.
        }

        private static float GetRotation(ItemFrameDirection direction)
        {
            switch (direction)
            {
                case ItemFrameDirection.North:
                    return 0;
                case ItemFrameDirection.South:
                    return 180;
                case ItemFrameDirection.West:
                    return 90;
                default:
                    return 270;
            }
        }

        public override byte EntityType
        {
            get { return 71; }
        }

        public override int Data
        {
            get { return (int)Direction; }
        }

        public override Size Size
        {
            get { return new Size(1, 1, 0.1); } // TODO: Oreintation
        }

        public override MetadataDictionary Metadata
        {
            get
            {
                var meta = base.Metadata;
                meta[2] = new MetadataSlot(2, Item);
                meta[3] = new MetadataByte(3, Orientation);
                return meta;
            }
        }

        public override bool IncludeMetadataOnClient
        {
            get { return true; }
        }

        public override void PhysicsUpdate(World world)
        {
            // No physics for this entity
        }

        public static ItemFrameDirection? Vector3ToDirection(Vector3 direction)
        {
            if (direction == Vector3.North)
                return ItemFrameDirection.North;
            if (direction == Vector3.South)
                return ItemFrameDirection.South;
            if (direction == Vector3.East)
                return ItemFrameDirection.East;
            if (direction == Vector3.West)
                return ItemFrameDirection.West;
            return null;
        }

        public enum ItemFrameDirection
        {
            South = 0,
            West = 1,
            North = 2,
            East = 3
        }
    }
}
