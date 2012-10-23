using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Entities
{
    public class BlockEntity : ObjectEntity
    {
        public BlockEntity(Vector3 position, Block block)
        {
            Position = position + new Vector3(0.5, 0, 0.5);
            Item = new Slot(block.Id, 1, block.Metadata);
        }

        public Slot Item { get; set; }

        public override Size Size
        {
            get { return new Size(1, 1, 1); }
        }

        public override byte EntityType
        {
            get { return 70; }
        }

        public override int Data
        {
            get { return Item.Id | (Item.Metadata << 0xC); }
        }

        public override float AccelerationDueToGravity
        {
            get { return 0.8f; }
        }

        public override float Drag
        {
            get
            {
                return base.Drag / 2;
            }
        }

        protected override void OnSleep(World world)
        {
            world.SetBlock(Position - new Vector3(0.5, -1, 0.5),
                Block.Create(Item.Id, (byte)Item.Metadata));
            world.OnDestroyEntity(this);
        }
    }
}
