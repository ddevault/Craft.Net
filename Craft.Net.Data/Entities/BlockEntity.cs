using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Events;

namespace Craft.Net.Data.Entities
{
    public class BlockEntity : ObjectEntity
    {
        public BlockEntity(Vector3 position, Block block)
        {
            Position = position + new Vector3(0.5, 0, 0.5);
            Item = new Slot(block.Id, 1, block.Metadata);
            TerrainCollision += BlockEntity_TerrainCollision;
        }

        public Slot Item { get; set; }

        public override Size Size
        {
            get { return new Size(0.9, 0.9, 0.9); } // TODO: See if this needs to be changed
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
            get { return 0.04f; }
        }

        public override float Drag
        {
            get
            {
                return 0.02f;
            }
        }

        void BlockEntity_TerrainCollision(object sender, EntityTerrainCollisionEventArgs e)
        {
            e.World.OnDestroyEntity(this);
            e.World.SetBlock(e.Block + Vector3.Up, (Block)Item.Id);
        }
    }
}
