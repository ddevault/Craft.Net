using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class JukeboxBlock : Block
    {
        public override ushort Id
        {
            get { return 84; }
        }

        public override double Hardness
        {
            get { return 2; }
        }
    }
}
