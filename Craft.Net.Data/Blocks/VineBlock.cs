using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class VineBlock : Block
    {
        public override ushort Id
        {
            get { return 106; }
        }

        public override double Hardness
        {
            get { return 0.2; }
        }

        public override bool CanHarvest(Items.ToolItem tool)
        {
            return false;
        }
    }
}
