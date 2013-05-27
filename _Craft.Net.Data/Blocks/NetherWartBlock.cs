using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
    public class NetherWartBlock : Block, IGrowableBlock
    {
        public static int MinimumGrowthTime = 30, MaximumGrowthTime = 120;

        public override short Id
        {
            get { return 115; }
        }

        public override BoundingBox? BoundingBox
        {
            get { return null; }
        }

        public override bool GetDrop(ToolItem tool, out ItemStack[] drop)
        {
            if (Metadata >= 3)
                drop = new[] { new ItemStack(new NetherWartItem(), (sbyte)MathHelper.Random.Next(2, 4)) };
            else
                drop = new[] { new ItemStack(new NetherWartItem()) };
            return true;
        }

        public override bool OnBlockPlaced(World world, Vector3 position, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entities.Entity usedBy)
        {
            var below = world.GetBlock(position + Vector3.Down);
            if (!(below is SoulSandBlock))
                return false;
            ScheduleGrowth(world, position);
            return base.OnBlockPlaced(world, position, clickedBlock, clickedSide, cursorPosition, usedBy);
        }

        private void ScheduleGrowth(World world, Vector3 position)
        {
            ScheduleUpdate(world, position, DateTime.Now.AddSeconds(MathHelper.Random.Next(MinimumGrowthTime, MaximumGrowthTime)));
        }

        public override void OnScheduledUpdate(World world, Vector3 position)
        {
            Grow(world, position, false);
        }

        public bool Grow(World world, Vector3 position, bool instant)
        {
            if (instant) return false;
            if (Metadata < 3)
            {
                Metadata++;
                world.SetBlock(position, this);
                ScheduleGrowth(world, position);
                return true;
            }
            return false;
        }
    }
}
