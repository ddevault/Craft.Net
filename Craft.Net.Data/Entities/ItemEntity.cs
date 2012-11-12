using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Events;

namespace Craft.Net.Data.Entities
{
    public class ItemEntity : Entity
    {
        public ItemEntity(Vector3 position, Slot item)
        {
            Position = position;
            Item = item;
            Velocity = new Vector3(
                (DataUtility.Random.NextDouble() - 0.5) * 0.1,
                (DataUtility.Random.NextDouble() - 0.5) * 0.1,
                (DataUtility.Random.NextDouble() - 0.5) * 0.1);
        }

        public override Size Size
        {
            get { return new Size(0.25, 0.25, 0.25); }
        }

        public Slot Item { get; set; }

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
            if (world.Level.Time % 10 == 0)
            {
                var player = (PlayerEntity)world.Entities.Where(e => e is PlayerEntity &&
                    e.Position.DistanceTo(Position) < 2).FirstOrDefault();
                if (player != null)
                    player.OnPickUpItem(new EntityEventArgs(this));
            }
        }
    }
}
