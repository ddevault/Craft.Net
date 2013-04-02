using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Entities;

namespace Craft.Net.Data.Blocks
{
    public class BurntOutTorchBlock : Block
    {
        public BurntOutTorchBlock()
        {
        }

        internal BurntOutTorchBlock(byte metadata)
        {
            Metadata = metadata;
        }

        public override short Id
        {
            get { return 161; }
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
                switch (this.Metadata)
                {
                    case 0x1:
                        return Vector3.West;
                    case 0x2:
                        return Vector3.East;
                    case 0x3:
                        return Vector3.North;
                    case 0x4:
                        return Vector3.South;
                    default:
                        return Vector3.Down;
                }
            }
        }

        public override bool OnBlockPlaced(World world, Vector3 position, Vector3 clickedBlock,
            Vector3 clickedSide, Vector3 cursorPosition, Entity usedBy)
        {
            if (clickedSide == Vector3.East)
                this.Metadata = 0x1;
            else if (clickedSide == Vector3.West)
                this.Metadata = 0x2;
            else if (clickedSide == Vector3.South)
                this.Metadata = 0x3;
            else if (clickedSide == Vector3.North)
                this.Metadata = 0x4;
            else if (clickedSide == Vector3.Up)
                this.Metadata = 0x5;
            else
                return false;
            return true;
        }
    }
}
