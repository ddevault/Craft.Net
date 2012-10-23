using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Entities
{
    public class ItemEntity : Entity
    {
        public ItemEntity(Vector3 position, Slot item)
        {
            Position = position;
            Item = item;
        }

        public override Size Size
        {
            get { return new Size(0.25, 0.25, 0.25); }
        }

        public Slot Item { get; set; }

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
    }
}
