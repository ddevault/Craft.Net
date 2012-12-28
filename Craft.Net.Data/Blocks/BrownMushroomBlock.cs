using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class BrownMushroomBlock : Block, IGrowableBlock
    {
        public override short Id
        {
            get { return 39; }
        }

        public void Grow(World world, Vector3 position)
        {
            // TODO
        }
    }
}
