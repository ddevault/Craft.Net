using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class HayBaleBlock : Block
    {
        // Taken from the orientation of wood, not sure if it's accurate yet
        public enum HayOrientation
        {
            Vertical = 0,
            EastWest = 0x4,
            NorthSouth = 0x8,
            None = 0xC
        }

        public HayOrientation Orientation
        {
            get { return (HayOrientation)(Metadata & 0xC); }
            set
            {
                Metadata &= unchecked((byte)~0xC);
                Metadata |= (byte)value;
            }
        }

        public override short Id
        {
            get { return 170; }
        }

        public override bool OnBlockPlaced(World world, Vector3 position, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entities.Entity usedBy)
        {
            if (clickedSide == Vector3.North || clickedSide == Vector3.South)
                Orientation = HayOrientation.NorthSouth;
            if (clickedSide == Vector3.East || clickedSide == Vector3.West)
                Orientation = HayOrientation.EastWest;
            return true;
        }
    }
}
