using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Entities;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
    public class PumpkinStemBlock : Block, IGrowableBlock
    {
        public static int MinimumGrowthSeconds = 30, MaximumGrowthSeconds = 120;

        public override short Id
        {
            get { return 104; }
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
            drop = new[] { new ItemStack(new PumpkinSeedsItem(), 1) };
            return true;
        }

        public override bool OnBlockPlaced(World world, Vector3 position, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entity usedBy)
        {
            var below = world.GetBlock(position + Vector3.Down);
            if (!(below is FarmlandBlock))
                return false;
            ScheduleGrowth(world, position);
            return base.OnBlockPlaced(world, position, clickedBlock, clickedSide, cursorPosition, usedBy);
        }

        public override void BlockUpdate(World world, Vector3 updatedBlock, Vector3 modifiedBlock)
        {
            if (!world.UpdatePending(updatedBlock))
            {
                var possibleLocations = new List<Vector3>(new[]
                {
                    updatedBlock + Vector3.Left, updatedBlock + Vector3.Right,
                    updatedBlock + Vector3.Forwards, updatedBlock + Vector3.Backwards
                });
                bool found = false;
                for (int i = 0; i < possibleLocations.Count; i++)
                {
                    if (world.GetBlock(possibleLocations[i]) is PumpkinBlock)
                        found = true;
                }
                if (!found)
                    ScheduleGrowth(world, updatedBlock);
            }
            base.BlockUpdate(world, updatedBlock, modifiedBlock);
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
                if (Metadata < 0x7)
                {
                    growth = true;
                    Metadata++;
                    ScheduleUpdate(world, position, DateTime.Now.AddSeconds(MathHelper.Random.Next(MinimumGrowthSeconds, MaximumGrowthSeconds)));
                }
                else if (Metadata == 0x7)
                {
                    // Spawn melon
                    // TODO: Is this the best way to do this?
                    var possibleLocations = new List<Vector3>(new[]
                {
                    position + Vector3.Left, position + Vector3.Right,
                    position + Vector3.Forwards, position + Vector3.Backwards
                });
                    for (int i = 0; i < possibleLocations.Count; i++)
                    {
                        var below = world.GetBlock(possibleLocations[i] + Vector3.Down);
                        if (!(world.GetBlock(possibleLocations[i]) is AirBlock) &&
                            (below is DirtBlock || below is GrassBlock))
                            possibleLocations.RemoveAt(i--);
                    }
                    if (possibleLocations.Count == 0)
                        ScheduleGrowth(world, position);
                    else
                        world.SetBlock(possibleLocations[MathHelper.Random.Next(possibleLocations.Count)], new PumpkinBlock());
                }
            }
            world.SetBlock(position, this);
            return growth;
        }
    }
}
