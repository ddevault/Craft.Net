using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class PistonHeadBlock : Block
    {
        public override short Id
        {
            get { return 34; }
        }

        public override double Hardness
        {
            get { return 0.5; } // TODO: This is a guess, burger doesn't report correctly
        }
    }
}
