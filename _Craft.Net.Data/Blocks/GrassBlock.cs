using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
    public class GrassBlock : Block, IGrowableBlock
    {
        public static int MinimumGrowthTime = 30, MaximumGrowthTime = 120;

        public override short Id
        {
            get { return 2; }
        }

        public override double Hardness
        {
            get { return 0.6; }
        }

        public override bool GetDrop(ToolItem tool, out ItemStack[] drop)
        {
            drop = new[] { new ItemStack(new DirtBlock(), 1) };
            return true;
        }

        public override string PlacementSoundEffect
        {
            get { return SoundEffect.DigGrass; }
        }

        public override bool OnBlockPlaced(World world, Vector3 position, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entities.Entity usedBy)
        {
            ScheduleGrowth(world, position);
            return base.OnBlockPlaced(world, position, clickedBlock, clickedSide, cursorPosition, usedBy);
        }

        public override void BlockUpdate(World world, Vector3 updatedBlock, Vector3 modifiedBlock)
        {
            if (!world.UpdatePending(updatedBlock))
                ScheduleGrowth(world, updatedBlock);
            base.BlockUpdate(world, updatedBlock, modifiedBlock);
        }

        public override void OnScheduledUpdate(World world, Vector3 position)
        {
            var block = world.GetBlock(position + Vector3.Up);
            if (block.LightReduction > 2)
                world.SetBlock(position, new DirtBlock());
            else
                Grow(world, position, false);
            base.OnScheduledUpdate(world, position);
        }

        private void ScheduleGrowth(World world, Vector3 position)
        {
            ScheduleUpdate(world, position, DateTime.Now.AddSeconds(MathHelper.Random.Next(MinimumGrowthTime, MaximumGrowthTime)));
        }

        public bool Grow(World world, Vector3 position, bool instant)
        {
            if (instant)
                return false; // TODO
            else
            {
                var possibleGrowth = new List<Vector3>();
                for (int y = -3; y <= 1; y++)
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int z = -1; z <= 1; z++)
                        {
                            var blockPosition = position + new Vector3(x, y, z);
                            var block = world.SafeGetBlock(blockPosition);
                            var above = world.SafeGetBlock(blockPosition + Vector3.Up);
                            if (block is DirtBlock && above is AirBlock)
                                possibleGrowth.Add(blockPosition);
                        }
                    }
                }
                if (!possibleGrowth.Any())
                    return false;
                var newPosition = possibleGrowth[MathHelper.Random.Next(possibleGrowth.Count)];
                world.SetBlock(newPosition, new GrassBlock());
                ScheduleGrowth(world, newPosition);
                return true;
            }
        }
    }
}
