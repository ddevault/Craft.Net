using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class DiamondOreBlock : Block
    {
        public override ushort Id
        {
            get { return 56; }
        }

        public override double Hardness
        {
            get { return 3; }
        }
    }
}
