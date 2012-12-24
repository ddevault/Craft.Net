using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class WallSignBlock : SignBlock
    {
        public WallSignBlock()
        {
        }

        public WallSignBlock(Direction direction)
        {
            Metadata = (byte)direction;
        }

        public override short Id
        {
            get { return 68; }
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
                switch (Metadata)
                {
                    case 0x02:
                        return Vector3.South;
                    case 0x03:
                        return Vector3.North;
                    case 0x04:
                        return Vector3.East;
                    case 0x05:
                        return Vector3.West;
                }
                return base.SupportDirection;
            }
        }
    }
}
