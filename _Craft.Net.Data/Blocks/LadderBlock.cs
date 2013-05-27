using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class LadderBlock : Block
    {
        public override short Id
        {
            get { return 65; }
        }

        public override double Hardness
        {
            get { return 0.4; }
        }

        public override BoundingBox? BoundingBox
        {
            get
            {
                switch (this.Metadata)
                {
                    case 2:
                        return new BoundingBox(new Vector3(0, 0, 1), new Vector3(1, 1, 0.9));
                    case 3:
                        return new BoundingBox(new Vector3(0, 0, 1), new Vector3(1, 1, 0.1));
                    case 4:
                        return new BoundingBox(new Vector3(0, 0, 0), new Vector3(0.1, 1, 1));
                    default:
                        return new BoundingBox(new Vector3(1, 0, 0), new Vector3(0.9, 1, 1));
                }
            }
        }

        public override bool OnBlockPlaced(World world, Vector3 position, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entities.Entity usedBy)
        {
            if (clickedSide == Vector3.South)      Metadata = 3;
            else if (clickedSide == Vector3.North) Metadata = 2;
            else if (clickedSide == Vector3.East)  Metadata = 5;
            else if (clickedSide == Vector3.West)  Metadata = 4;
            else return false;
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
                switch (this.Metadata)
                {
                    case 2:  return Vector3.South;
                    case 3:  return Vector3.North;
                    case 4:  return Vector3.East;
                    default: return Vector3.West;
                }
            }
        }
    }
}
