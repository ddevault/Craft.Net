using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public enum StairDirections
    {
        East = 0x0,
        West = 0x1,
        South = 0x2,
        North = 0x3
    }

    public abstract class StairBlock : Block
    {
        public override double Hardness
        {
            get { return 2; }
        }

        public override bool OnBlockPlaced(World world, Vector3 position, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entities.Entity usedBy)
        {
            Metadata = (byte)MathHelper.DirectionByRotationFlat(usedBy);
            switch ((Direction)Metadata)
            {
                case Direction.North:
                    Metadata = (byte)StairDirections.North;
                    break;
                case Direction.South:
                    Metadata = (byte)StairDirections.South;
                    break;
                case Direction.West:
                    Metadata = (byte)StairDirections.West;
                    break;
                case Direction.East:
                    Metadata = (byte)StairDirections.East;
                    break;
            }
            if (clickedSide == Vector3.Down)
                Metadata |= 4;
            else if (clickedSide != Vector3.Up)
            {
                if (cursorPosition.Y >= 8)
                    Metadata |= 4;
            }
            return true;
        }
    }
}
