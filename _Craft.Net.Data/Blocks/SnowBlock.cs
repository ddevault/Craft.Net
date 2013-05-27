using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class SnowBlock : Block
    {
        public override short Id
        {
            get { return 80; }
        }

        public override double Hardness
        {
            get { return 0.2; }
        }
    }
}
