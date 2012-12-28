using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Entities;

namespace Craft.Net.Data.Blocks
{
    public class CactusBlock : Block, IGrowableBlock
    {
        public static int MinimumGrowthSeconds = 30, MaximumGrowthSeconds = 120;

        public override short Id
        {
            get { return 81; }
        }

        public override double Hardness
        {
            get { return 0.4; }
        }

        public override bool OnBlockPlaced(World world, Vector3 position, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entities.Entity usedBy)
        {
            var below = world.GetBlock(position + Vector3.Down);
            ScheduleGrowth(world, position);
            return below is SandBlock || below is CactusBlock;
        }

        public override void OnScheduledUpdate(World world, Vector3 position)
        {
            Grow(world, position);
            base.OnScheduledUpdate(world, position);
        }

        public override void BlockUpdate(World world, Vector3 updatedBlock, Vector3 modifiedBlock)
        {
            var below = world.GetBlock(updatedBlock + Vector3.Down);
            if (!(below is SandBlock || below is CactusBlock))
            {
                world.SetBlock(updatedBlock, new AirBlock());
                var entity = new ItemEntity(updatedBlock + new Vector3(0.5), new ItemStack(this));
                entity.ApplyRandomVelocity();
                world.OnSpawnEntity(entity);
            }
            base.BlockUpdate(world, updatedBlock, modifiedBlock);
        }

        private void ScheduleGrowth(World world, Vector3 position)
        {
            ScheduleUpdate(world, position, DateTime.Now.AddSeconds(MathHelper.Random.Next(MinimumGrowthSeconds, MaximumGrowthSeconds)));
        }

        public void Grow(World world, Vector3 position)
        {
            if (world.GetBlock(position + Vector3.Up) is AirBlock)
            {
                int cacti = 1;
                for (int y = 1; y < 3; y++)
                {
                    if (world.GetBlock(position - new Vector3(0, y, 0)) is CactusBlock)
                        cacti++;
                    else
                        break;
                }
                if (cacti < 3)
                {
                    world.SetBlock(position + Vector3.Up, new CactusBlock());
                    ScheduleGrowth(world, position + Vector3.Up);
                }
            }
        }
    }
}
