using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Entities;

namespace Craft.Net.Data.Blocks
{
    public class LeverBlock : Block
    {
        public override short Id
        {
            get { return 69; }
        }

        public override double Hardness
        {
            get { return 0.5; }
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
            get 
            {
                switch (Metadata & 0xF7)
                {
                    case 5:
                    case 6: return Vector3.Down;
                    case 0:
                    case 7: return Vector3.Up;
                    case 1: return Vector3.West;
                    case 2: return Vector3.East;
                    case 3: return Vector3.North;
                    case 4: return Vector3.South;
                    default: return Vector3.Down;
                }
            }
        }

        public override bool OnBlockPlaced(World world, Vector3 position, Vector3 clickedBlock,
            Vector3 clickedSide, Vector3 cursorPosition, Entity usedBy)
        {
            if (clickedSide == Vector3.Up)
                this.Metadata = 5; // TODO: Orientation
            else if (clickedSide == Vector3.Down)
                this.Metadata = 0;
            else
            {
                if (clickedSide == Vector3.East)  this.Metadata = 1;
                if (clickedSide == Vector3.West)  this.Metadata = 2;
                if (clickedSide == Vector3.South) this.Metadata = 3;
                if (clickedSide == Vector3.North) this.Metadata = 4;
            }
            return true;
        }

        public override bool OnBlockRightClicked(Vector3 clickedBlock, Vector3 clickedSide,
            Vector3 cursorPosition, World world, Entity usedBy)
        {
            Metadata ^= 0x8;
            world.SetBlock(clickedBlock, this);
            return false;
        }
    }
}
