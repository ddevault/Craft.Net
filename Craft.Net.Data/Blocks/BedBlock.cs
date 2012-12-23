using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Entities;

namespace Craft.Net.Data.Blocks
{
    public enum BedDirection
    {
        South = 0,
        West = 1,
        North = 2,
        East = 3
    }

    public class BedBlock : Block
    {
        public bool IsHeadboard
        {
            get { return (Metadata & 0x8) == 0x8; }
            set 
            {
                Metadata &= unchecked((byte)~0x8);
                if (value)
                    Metadata |= 0x8;
            }
        }

        public bool IsOccupied
        {
            get { return (Metadata & 0x4) == 0x4; }
            set
            {
                Metadata &= unchecked((byte)~0x3);
                if (value)
                    Metadata |= 0x4;
            }
        }

        public BedDirection Direction
        {
            get { return (BedDirection) (Metadata & unchecked((byte) ~0x4)); }
            set
            {
                Metadata &= unchecked((byte)~0x4);
                Metadata |= (byte)value;
            }
        }

        public BedBlock()
        {
        }

        public BedBlock(BedDirection direction, bool isHeadboard)
        {
            IsHeadboard = isHeadboard;
            Direction = direction;
        }

        public override short Id
        {
            get { return 26; }
        }

        public override double Hardness
        {
            get { return 0.2; }
        }

        public override bool OnBlockRightClicked(Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, World world, Entities.Entity usedBy)
        {
            if (IsOccupied)
                return false;
            IsOccupied = true;
            var entity = (PlayerEntity)usedBy;
            if (!IsHeadboard)
                clickedBlock += BedDirectionToVector3(Direction);
            entity.EnterBed(clickedBlock);
            return false;
        }

        public static BedDirection Vector3ToBedDirection(Vector3 direction)
        {
            if (direction == Vector3.East)
                return BedDirection.East;
            if (direction == Vector3.North)
                return BedDirection.North;
            if (direction == Vector3.South)
                return BedDirection.South;
            return BedDirection.West;
        }

        public static Vector3 BedDirectionToVector3(BedDirection direction)
        {
            switch (direction)
            {
                case BedDirection.North:
                    return Vector3.North;
                case BedDirection.South:
                    return Vector3.South;
                case BedDirection.West:
                    return Vector3.West;
                default:
                    return Vector3.East;
            }
        }
    }
}
