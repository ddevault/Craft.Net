using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class LadderBlock : Block
    {
        public override ushort Id
        {
            get { return 65; }
        }

        public override double Hardness
        {
            get { return 0.4; }
        }

        public override bool OnBlockPlaced(World world, Vector3 position, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entities.Entity usedBy)
        {
            this.Metadata = (byte)DataUtility.DirectionByRotationFlat(usedBy, true);
            return true;
        }
    }
}
