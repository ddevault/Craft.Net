using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Entities;

namespace Craft.Net.Data.Blocks
{
    public class WoodBlock : Block
    {
        public enum TreeType
        {
            Oak = 0,
            Spruce = 1,
            Birch = 2,
            Jungle = 3
        }

        public enum TreeOrientation
        {
            Vertical = 0,
            EastWest = 0x4,
            NorthSouth = 0x8,
            None = 0xC
        }

        public TreeType Type
        {
            get { return (TreeType)(Metadata & 3); }
            set
            {
                Metadata &= unchecked((byte)~3);
                Metadata |= (byte)value;
            }
        }

        public TreeOrientation Orientation
        {
            get { return (TreeOrientation)(Metadata & 0xC); }
            set
            {
                Metadata &= unchecked((byte)~0xC);
                Metadata |= (byte)value;
            }
        }

        public override short Id
        {
            get { return 17; }
        }

        public override double Hardness
        {
            get { return 2; }
        }

        public override bool OnBlockPlaced(World world, Vector3 position, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entities.Entity usedBy)
        {
            if (clickedSide == Vector3.North || clickedSide == Vector3.South)
                Orientation = TreeOrientation.NorthSouth;
            if (clickedSide == Vector3.East || clickedSide == Vector3.West)
                Orientation = TreeOrientation.EastWest;
            return true;
        }
    }
}
