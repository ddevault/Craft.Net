using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class NetherWartBlock : Block, IGrowableBlock
    {
        public override short Id
        {
            get { return 115; }
        }

        public override BoundingBox? BoundingBox
        {
            get { return null; }
        }

        public bool Grow(World world, Vector3 position, bool instant)
        {
            // TODO
            return false;
        }
    }
}
