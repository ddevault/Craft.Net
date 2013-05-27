using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Entities;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
    public class PotatoBlock : Block, IGrowableBlock
    {
        public static int MinimumGrowthSeconds = 30, MaximumGrowthSeconds = 120;

        public override short Id
        {
            get { return 142; }
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
            if (Metadata == 7)
            {
                if (MathHelper.Random.Next(100) == 0)
                {
                    drop = new[] { new ItemStack(new PotatoItem(), (sbyte)MathHelper.Random.Next(1, 4)),
                    new ItemStack(new PoisonousPotatoItem(), 1) };
                }
                else
                    drop = new[] { new ItemStack(new PotatoItem(), (sbyte)MathHelper.Random.Next(1, 4)) };
            }
            else
                drop = new[] { new ItemStack(new PotatoItem()) };
            return true;
        }

        public override bool OnBlockPlaced(World world, Vector3 position, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entity usedBy)
        {
            ScheduleGrowth(world, position);
            return true;
        }

        public override void BlockUpdate(World world, Vector3 updatedBlock, Vector3 modifiedBlock)
        {
            if (!(world.GetBlock(updatedBlock + Vector3.Down) is FarmlandBlock))
            {
                var entity = new ItemEntity(updatedBlock + new Vector3(0.5), new ItemStack(new PotatoItem()));
                entity.ApplyRandomVelocity();
                world.SetBlock(updatedBlock, new AirBlock());
                world.OnSpawnEntity(entity);
            }
            base.BlockUpdate(world, updatedBlock, modifiedBlock);
        }

        private void ScheduleGrowth(World world, Vector3 position)
        {
            ScheduleUpdate(world, position, DateTime.Now.AddSeconds(MathHelper.Random.Next(MinimumGrowthSeconds, MaximumGrowthSeconds)));
        }

        public override void OnScheduledUpdate(World world, Vector3 position)
        {
            Grow(world, position, false);
            base.OnScheduledUpdate(world, position);
        }

        public bool Grow(World world, Vector3 position, bool instant)
        {
            bool growth = false;
            if (instant)
            {
                growth = Metadata != 0x7;
                Metadata = 0x7;
            }
            else
            {
                Metadata++;
                growth = true;
                if (Metadata != 7)
                    ScheduleGrowth(world, position);
            }
            world.SetBlock(position, this);
            return growth;
        }
    }
}
