using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class NetherBrickBlock : Block
    {
        public override ushort Id
        {
            get { return 112; }
        }

        public override double Hardness
        {
            get { return 2; }
        }
    }
}
