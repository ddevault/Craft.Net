using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Items;
using Craft.Net.Data.Entities;

namespace Craft.Net.Data.Blocks
{
    public class SeedsBlock : Block
    {
        public override short Id
        {
            get { return 59; }
        }

        public override BoundingBox? BoundingBox
        {
            get { return null; }
        }

        public override bool RequiresSupport
        {
            get { return true; }
        }

        public override Vector3 SupportDirection
        {
            get { return Vector3.Down; }
        }

        public override bool GetDrop(ToolItem tool, out ItemStack[] drop)
        {
            if (Metadata != 7)
                drop = new[] { new ItemStack(new SeedsItem(), 1) };
            else
                drop = new[] { new ItemStack(new WheatItem(), 1), new ItemStack(new SeedsItem(), (sbyte)MathHelper.Random.Next(1, 4)) };
            return true;
        }

        public override bool OnBlockPlaced(World world, Vector3 position, Vector3 clickedBlock,
            Vector3 clickedSide, Vector3 cursorPosition, Entity usedBy)
        {
            ScheduleUpdate(world, position, DateTime.Now.AddSeconds(1));
            return true;
        }

        public override void OnScheduledUpdate(World world, Vector3 position)
        {
            Metadata++;
            if (Metadata != 7)
                ScheduleUpdate(world, position, DateTime.Now.AddSeconds(1));
            world.SetBlock(position, this);
            base.OnScheduledUpdate(world, position);
        }
    }
}
