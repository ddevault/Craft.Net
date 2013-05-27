using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class JackOLanternBlock : Block
    {
        public override short Id
        {
            get { return 91; }
        }

        public override double Hardness
        {
            get { return 1; }
        }

        public override bool OnBlockPlaced(World world, Vector3 position, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entities.Entity usedBy)
        {
            return true;
        }
    }
}
