using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class StoneBrickBlock : Block
    {
        public override short Id
        {
            get { return 98; }
        }

        public override double Hardness
        {
            get { return 1.5; }
        }
    }
}
