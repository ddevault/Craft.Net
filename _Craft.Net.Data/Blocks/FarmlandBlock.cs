using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Entities;

namespace Craft.Net.Data.Blocks
{
    public class FarmlandBlock : Block
    {
        public override short Id
        {
            get { return 60; }
        }

        public override double Hardness
        {
            get { return 0.6; }
        }

        public override bool GetDrop(Items.ToolItem tool, out ItemStack[] drop)
        {
            drop = new[] { new ItemStack(new DirtBlock(), 1) };
            return true;
        }

        public override bool OnBlockPlaced(World world, Vector3 position, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entity usedBy)
        {
            ScheduleUpdate(world, position, DateTime.Now.AddSeconds(MathHelper.Random.Next(30, 120)));
            return true;
        }

        public override void OnScheduledUpdate(World world, Vector3 position)
        {
            // Check for water
            int closest = int.MaxValue;
            for (int y = -1; y < 1; y++ )
                for (int x = -8; x < 8; x++)
                    for (int z = -8; z < 8; z++)
                    {
                        if (position.Y + y < 0)
                            continue;
                        var blockPosition = new Vector3(x, position.Y + y, z);
                        var block = world.GetBlock(blockPosition);
                        if (block is WaterFlowingBlock || block is WaterStillBlock)
                        {
                            var distance = position.DistanceTo(blockPosition);
                            if (distance < closest)
                                closest = (int)distance;
                        }
                    }
            closest = -(closest - 4) + 4;
            Metadata = (byte)closest;
            if (closest >= 5 && MathHelper.Random.Next(5) == 0)
                world.SetBlock(position, new DirtBlock());
            else
            {
                world.SetBlock(position, this);
                ScheduleUpdate(world, position, DateTime.Now.AddSeconds(MathHelper.Random.Next(30, 120)));
            }
            base.OnScheduledUpdate(world, position);
        }

        public override void BlockUpdate(World world, Vector3 updatedBlock, Vector3 modifiedBlock)
        {
            var block = world.GetBlock(updatedBlock + Vector3.Up);
            if (!(block is AirBlock) && !(block is IGrowableBlock))
                world.SetBlock(updatedBlock, new DirtBlock());
            base.BlockUpdate(world, updatedBlock, modifiedBlock);
        }

        public override void OnBlockWalkedOn(World world, Vector3 position, Entity entity)
        {
            if (entity.Velocity.Y < -0.3)
                world.SetBlock(position, new DirtBlock());
        }
    }
}
