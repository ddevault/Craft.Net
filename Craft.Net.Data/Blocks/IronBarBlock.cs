using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class IronBarBlock : Block
    {
        public override ushort Id
        {
            get { return 101; }
        }

        public override double Hardness
        {
            get { return 5; }
        }
    }
}
