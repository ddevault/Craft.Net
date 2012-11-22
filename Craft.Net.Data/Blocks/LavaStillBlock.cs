using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class LavaStillBlock : Block
    {
        public override ushort Id
        {
            get { return 11; }
        }

        public override double Hardness
        {
            get { return 100; }
        }
    }
}