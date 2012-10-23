using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Entities;

namespace Craft.Net.Data.Blocks
{
    public class PistonBlock : Block
    {
        public override ushort Id
        {
            get { return 33; }
        }

        public override double Hardness
        {
            get { return 0.5; }
        }

        public override bool OnBlockPlaced(World world, Vector3 position, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entity usedBy)
        {
            this.Metadata = (byte)DataUtility.DirectionByRotation((PlayerEntity)usedBy, position, true);
            return true;
        }
    }
}
