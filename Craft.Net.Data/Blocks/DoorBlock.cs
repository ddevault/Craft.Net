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
    }
}
