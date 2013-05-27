using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Entities;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
    public class SugarCaneBlock : Block, IGrowableBlock
    {
        public static int MinimumGrowthSeconds = 30, MaximumGrowthSeconds = 120;

        public override short Id
        {
            get { return 83; }
        }

        public override BoundingBox? BoundingBox
        {
            get { return null; }
        }

        public override bool GetDrop(ToolItem tool, out ItemStack[] drop)
        {
            drop = new[] { new ItemStack(new SugarCanesItem(), 1) };
            return true;
        }

        public override bool OnBlockPlaced(World world, Vector3 position, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entities.Entity usedBy)
        {
            var bottom = world.GetBlock(position + Vector3.Down);
            if (bottom is SugarCaneBlock)
            {
                ScheduleUpdate(world, position,
                    DateTime.Now.AddSeconds(MathHelper.Random.Next(MinimumGrowthSeconds, MaximumGrowthSeconds)));
                return true;
            }
            if (!(bottom is DirtBlock || bottom is GrassBlock || bottom is SandBlock))
                return false;
            // Look for water
            bool found = false;
            for (int x = -1; x <= 1; x++)
                for (int z = -1; z <= 1; z++)
                {
                    var block = world.GetBlock(position + new Vector3(x, -1, z));
                    if (block is WaterFlowingBlock || block is WaterStillBlock)
                    {
                        found = true;
                        break;
                    }
                }
            if (found)
            {
                ScheduleGrowth(world, position);
                return true;
            }
            return false;
        }

        public override void OnScheduledUpdate(World world, Vector3 position)
        {
            Grow(world, position, false);
            base.OnScheduledUpdate(world, position);
        }

        private void ScheduleGrowth(World world, Vector3 position)
        {
            ScheduleUpdate(world, position, DateTime.Now.AddSeconds(MathHelper.Random.Next(MinimumGrowthSeconds, MaximumGrowthSeconds)));
        }

        public override void BlockUpdate(World world, Vector3 updatedBlock, Vector3 modifiedBlock)
        {
            var block = world.GetBlock(updatedBlock + Vector3.Down);
            if (!(block is SugarCaneBlock || block is DirtBlock || block is GrassBlock || block is SandBlock))
            {
                world.SetBlock(updatedBlock, new AirBlock());
                var entity = new ItemEntity(updatedBlock + new Vector3(0.5), new ItemStack(new SugarCanesItem()));
                entity.ApplyRandomVelocity();
                world.OnSpawnEntity(entity);
            }
            base.BlockUpdate(world, updatedBlock, modifiedBlock);
        }

        public bool Grow(World world, Vector3 position, bool instant)
        {
            // Get stack height
            if (!instant)
            {
                if (world.GetBlock(position + Vector3.Up) is AirBlock)
                {
                    int reeds = 1;
                    for (int y = 1; y < 3; y++)
                    {
                        if (world.GetBlock(position - new Vector3(0, y, 0)) is SugarCaneBlock)
                            reeds++;
                        else
                            break;
                    }
                    if (reeds < 3)
                    {
                        world.SetBlock(position + Vector3.Up, new SugarCaneBlock());
                        ScheduleGrowth(world, position + Vector3.Up);
                    }
                }
            }
            return !instant;
        }
    }
}
