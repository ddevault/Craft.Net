using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class NetherBrickFenceBlock : Block
    {
        public override ushort Id
        {
            get { return 113; }
        }

        public override double Hardness
        {
            get { return 2; }
        }
    }
}
