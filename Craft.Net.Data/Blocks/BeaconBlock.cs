using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class BeaconBlock : Block
    {
        public override ushort Id
        {
            get { return 138; }
        }

        public override double Hardness
        {
            get { return 1.5; } // TODO: Estimated, burger reports 0
        }

        public override bool OnBlockRightClicked(Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, World world, Entities.Entity usedBy)
        {
            return false;
        }
    }
}
