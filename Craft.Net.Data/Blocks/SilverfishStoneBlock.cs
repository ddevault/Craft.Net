using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class SilverfishStoneBlock : Block
    {
        public override ushort Id
        {
            get { return 97; }
        }

        public override double Hardness
        {
            get { return 0.75; }
        }
    }
}
