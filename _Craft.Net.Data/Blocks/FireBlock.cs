using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class FireBlock : Block
    {
        public override short Id
        {
            get { return 51; }
        }

        public override BoundingBox? BoundingBox
        {
            get { return null; }
        }

        public override bool CanHarvest(Items.ToolItem tool)
        {
            return false;
        }
    }
}
