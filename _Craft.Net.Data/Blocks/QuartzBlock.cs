using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class QuartzBlock : Block
    {
        public enum QuartzType
        {
            Block = 0,
            Chiseled = 1,
            Pillar = 2,
            PillarEastWest = 3,
            PillarNorthSouth = 4
        }

        public QuartzType Type
        {
            get { return (QuartzType)(Metadata & 3); }
            set
            {
                Metadata &= unchecked((byte)~3);
                Metadata |= (byte)value;
            }
        }

        public override short Id
        {
            get { return 155; }
        }

        public override bool OnBlockPlaced(World world, Vector3 position, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entities.Entity usedBy)
        {
            if (Type == QuartzType.Pillar)
            {
                if (clickedSide == Vector3.North || clickedSide == Vector3.South)
                    Type = QuartzType.PillarNorthSouth;
                if (clickedSide == Vector3.East || clickedSide == Vector3.West)
                    Type = QuartzType.PillarEastWest;
            }
            return true;
        }
    }
}
