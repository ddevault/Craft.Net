using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class StoneButtonBlock : Block
    {
        public override ushort Id
        {
            get { return 77; }
        }

        public override double Hardness
        {
            get { return 0.5; }
        }
    }
}
