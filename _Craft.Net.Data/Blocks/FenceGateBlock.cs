using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class FenceGateBlock : Block
    {
        public override short Id
        {
            get { return 107; }
        }

        public override double Hardness
        {
            get { return 2; }
        }

        public override bool OnBlockRightClicked(Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, World world, Entities.Entity usedBy)
        {
            return false;
        }
    }
}
