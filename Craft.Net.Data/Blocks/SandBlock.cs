using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class SandBlock : Block
    {
        public override ushort Id
        {
            get { return 12; }
        }

        public override double Hardness
        {
            get { return 0.5; }
        }
    }
}
