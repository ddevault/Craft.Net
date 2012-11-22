using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class SlabBlock : Block
    {
        public override ushort Id
        {
            get { return 44; }
        }

        public override double Hardness
        {
            get { return 2; }
        }
    }
}