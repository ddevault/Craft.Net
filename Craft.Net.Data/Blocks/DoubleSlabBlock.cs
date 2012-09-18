using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class DoubleSlabBlock : Block
    {
        public override ushort Id
        {
            get { return 43; }
        }

        public override double Hardness
        {
            get { return 2; }
        }
    }
}
