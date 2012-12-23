using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class WaterStillBlock : Block
    {
        public override short Id
        {
            get { return 9; }
        }

        public override double Hardness
        {
            get { return 100; }
        }
    }
}
