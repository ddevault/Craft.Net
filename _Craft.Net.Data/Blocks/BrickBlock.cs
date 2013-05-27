using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class BrickBlock : Block
    {
        public override short Id
        {
            get { return 45; }
        }

        public override double Hardness
        {
            get { return 2; }
        }
    }
}
