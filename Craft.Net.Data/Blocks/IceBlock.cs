using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class IceBlock : Block
    {
        public override ushort Id
        {
            get { return 79; }
        }

        public override double Hardness
        {
            get { return 0.5; }
        }
    }
}
