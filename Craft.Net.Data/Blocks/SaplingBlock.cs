using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class SaplingBlock : Block
    {
        public override short Id
        {
            get { return 6; }
        }

        public override BoundingBox? BoundingBox
        {
            get { return null; }
        }
    }
}
