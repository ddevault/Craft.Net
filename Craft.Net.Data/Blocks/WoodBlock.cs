using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Entities;

namespace Craft.Net.Data.Blocks
{
    public class WoodBlock : Block
    {
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
            var direction = (Direction)MathHelper.DirectionByRotation((PlayerEntity)usedBy, position, true);
            switch (direction)
            {
                case Direction.North:
                case Direction.South:
                    this.Metadata |= 0x4;
                    break;
                case Direction.East:
                case Direction.West:
                    this.Metadata |= 0x8;
                    break;
            }
            return true;
        }
    }
}
