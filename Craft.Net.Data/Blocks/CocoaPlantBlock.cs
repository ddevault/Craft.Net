using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
    public class CocoaPlantBlock : Block, IGrowableBlock
    {
        public static int MinimumGrowthSeconds = 30, MaximumGrowthSeconds = 120;

        public enum CocoaPlantDirection
        {
            North = 0,
            East = 1,
            South = 2,
            West = 3
        }

        public enum CocoaPlantSize
        {
            Small = 0,
            Medium = 0x4,
            Large = 0x8,
            //Enderdragon = 0xC
        }

        public CocoaPlantDirection Direction
        {
            get { return (CocoaPlantDirection)(Metadata & 3); }
            set
            {
                Metadata &= unchecked((byte)~3);
                Metadata |= (byte)value;
            }
        }

        public CocoaPlantSize Size
        {
            get { return (CocoaPlantSize)(Metadata & 0xC); }
            set
            {
                Metadata &= unchecked((byte)~0xC);
                Metadata |= (byte)value;
            }
        }

        public override short Id
        {
            get { return 127; }
        }

        public override double Hardness
        {
            get { return 0.2; }
        }

        public override BoundingBox? BoundingBox
        {
            get { return null; }
        }

        public override bool OnBlockPlaced(World world, Vector3 position, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entities.Entity usedBy)
        {
            var block = world.GetBlock(clickedBlock) as WoodBlock;
            if (block == null)
                return false;
            if (block.Type != WoodBlock.TreeType.Jungle)
                return false;
            ScheduleGrowth(world, position);
            return true;
        }

        public override void OnScheduledUpdate(World world, Vector3 position)
        {
            Grow(world, position);
            base.OnScheduledUpdate(world, position);
        }

        private void ScheduleGrowth(World world, Vector3 position)
        {
            ScheduleUpdate(world, position, DateTime.Now.AddSeconds(MathHelper.Random.Next(MinimumGrowthSeconds, MaximumGrowthSeconds)));
        }

        public override bool GetDrop(ToolItem tool, out ItemStack[] drop)
        {
            if (Size != CocoaPlantSize.Large)
                drop = new[] { new ItemStack(new DyeItem(), 1, (short)DyeItem.DyeType.CocoaBeans) };
            else
                drop = new[] { new ItemStack(new DyeItem(), 3, (short)DyeItem.DyeType.CocoaBeans) };
            return true;
        }

        public override bool RequiresSupport
        {
            get { return true; }
        }

        public override Vector3 SupportDirection
        {
            get
            {
                if (Direction == CocoaPlantDirection.North)
                    return Vector3.South;
                if (Direction == CocoaPlantDirection.South)
                    return Vector3.North;
                if (Direction == CocoaPlantDirection.East)
                    return Vector3.West;
                return Vector3.East;
            }
        }

        public void Grow(World world, Vector3 position)
        {
            if (Size == CocoaPlantSize.Small) Size = CocoaPlantSize.Medium;
            else if (Size == CocoaPlantSize.Medium) Size = CocoaPlantSize.Large;
            world.SetBlock(position, this);
            if (Size != CocoaPlantSize.Large)
                ScheduleGrowth(world, position);
        }
    }
}
