using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class WoodenDoubleSlabBlock : Block
    {
        public override ushort Id
        {
            get { return 125; }
        }

        public override double Hardness
        {
            get { return 2; }
        }
    }
}
