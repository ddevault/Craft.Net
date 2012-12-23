using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class SandstoneBlock : Block
    {
        public override short Id
        {
            get { return 24; }
        }

        public override double Hardness
        {
            get { return 0.8; }
        }
    }
}
