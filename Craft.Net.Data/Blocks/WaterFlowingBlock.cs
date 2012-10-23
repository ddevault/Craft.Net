using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class WaterFlowingBlock : Block
    {
        public override ushort Id
        {
            get { return 8; }
        }

        public override double Hardness
        {
            get { return 100; } // TODO: -1? Burger reports 100
        }
    }
}
