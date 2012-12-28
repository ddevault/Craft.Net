using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Entities;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
    public class VineBlock : Block, IGrowableBlock
    {
        public static int MinimumGrowthSeconds = 30, MaximumGrowthSeconds = 120;

        public enum VineDirection
        {
            North = 1,
            East = 2,
            South = 4,
            West = 8
        }

        public VineBlock() { }

        public VineBlock(VineDirection direction)
        {
            Direction = direction;
        }

        public VineDirection Direction
        {
            get { return (VineDirection)Metadata; }
            set { Metadata = (byte)value; }
        }

        public override short Id
        {
            get { return 106; }
        }

        public override double Hardness
        {
            get { return 0.2; }
        }

        public override bool CanHarvest(ToolItem tool)
        {
            return tool is ShearsItem;
        }

        public override bool OnBlockPlaced(World world, Vector3 position, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entity usedBy)
        {
            var player = usedBy as PlayerEntity;
            var support = world.GetBlock(clickedBlock);
            if (support is AirBlock)
                return false;
            if (clickedSide == Vector3.North)
                Direction = VineDirection.North;
            else if (clickedSide == Vector3.South)
                Direction = VineDirection.South;
            else if (clickedSide == Vector3.West)
                Direction = VineDirection.West;
            else if (clickedSide == Vector3.East)
                Direction = VineDirection.East;
            else
                return false;
            ScheduleGrowth(world, position);
            return true;
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

        public override void BlockUpdate(World world, Vector3 updatedBlock, Vector3 modifiedBlock)
        {
            Vector3 direction;
            switch (Direction)
            {
                // Inverted
                case VineDirection.North: direction = Vector3.South; break;
                case VineDirection.South: direction = Vector3.North; break;
                case VineDirection.East:  direction = Vector3.West;  break;
                default:                  direction = Vector3.East;  break;
            }
            var support = world.GetBlock(updatedBlock + direction);
            if (support is AirBlock)
            {
                var above = world.GetBlock(updatedBlock + Vector3.Up);
                if (!(above is VineBlock))
                    world.SetBlock(updatedBlock, new AirBlock());
            }
            base.BlockUpdate(world, updatedBlock, modifiedBlock);
        }

        public bool Grow(World world, Vector3 position, bool instant)
        {
            // Grow downwards
            if (!instant)
            {
                var block = world.GetBlock(position + Vector3.Down);
                if (block is AirBlock)
                {
                    world.SetBlock(position + Vector3.Down, new VineBlock(Direction));
                    ScheduleGrowth(world, position + Vector3.Down);
                }
            }
            return !instant;
        }
    }
}
