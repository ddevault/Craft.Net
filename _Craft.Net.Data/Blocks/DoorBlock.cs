using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public enum DoorDirection
    {
        West = 0,
        North = 1,
        East = 2,
        South = 3
    }

    public abstract class DoorBlock : Block
    {
        public bool UpperHalf
        {
            get { return (Metadata & 0x8) == 0x8; }
            set
            {
                Metadata &= unchecked((byte)~0x8);
                if (value)
                    Metadata |= 0x8;
            }
        }

        public bool LeftHinge
        {
            get { return (Metadata & 0x1) == 0x1; }
            set
            {
                Metadata &= unchecked((byte)~0x1);
                if (value)
                    Metadata |= 0x1;
            }
        }

        public bool Closed
        {
            get { return (Metadata & 0x4) == 0x4; }
            set
            {
                Metadata &= unchecked((byte)~0x4);
                if (value)
                    Metadata |= 0x4;
            }
        }

        public DoorDirection Direction
        {
            get { return (DoorDirection)(Metadata & 0x3); }
            set
            {
                Metadata &= unchecked((byte)~0x3);
                Metadata |= (byte)value;
            }
        }

        public static DoorDirection Vector3ToDoorDirection(Vector3 direction)
        {
            if (direction == Vector3.East)
                return DoorDirection.East;
            if (direction == Vector3.North)
                return DoorDirection.North;
            if (direction == Vector3.South)
                return DoorDirection.South;
            return DoorDirection.West;
        }

        public static Vector3 DoorDirectionToVector3(DoorDirection direction)
        {
            if (direction == DoorDirection.East)
                return Vector3.East;
            if (direction == DoorDirection.North)
                return Vector3.North;
            if (direction == DoorDirection.West)
                return Vector3.West;
            return Vector3.East;
        }

        public override void BlockUpdate(World world, Vector3 updatedBlock, Vector3 modifiedBlock)
        {
            if (UpperHalf)
            {
                var block = world.GetBlock(updatedBlock + Vector3.Down) as DoorBlock;
                if (block == null)
                    world.SetBlock(updatedBlock, new AirBlock());
                else
                {
                    var left = world.GetBlock(updatedBlock + DoorDirectionToVector3(block.Direction + 1 % 4) + Vector3.Down) as DoorBlock;
                    if (left != null)
                    {
                        if (left.Direction == block.Direction)
                        {
                            world.EnableBlockUpdates = false;
                            LeftHinge = true;
                            world.SetBlock(updatedBlock, this);
                            world.EnableBlockUpdates = true;
                        }
                    }
                }
            }
            else
            {
                var block = world.GetBlock(updatedBlock + Vector3.Up);
                if (!(block is DoorBlock))
                    world.SetBlock(updatedBlock, new AirBlock());
            }
            base.BlockUpdate(world, updatedBlock, modifiedBlock);
        }

        public override bool RequiresSupport
        {
            get { return !UpperHalf; }
        }

        public override Vector3 SupportDirection
        {
            get { return Vector3.Down; }
        }
    }
}
