using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class NetherPortalBlock : Block
    {
        public override short Id
        {
            get { return 90; }
        }

        public override double Hardness
        {
            get { return -1; }
        }

        public override BoundingBox? BoundingBox
        {
            get { return null; }
        }
    }
}
