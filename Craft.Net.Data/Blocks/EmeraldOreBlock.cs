using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class EmeraldOreBlock : Block
    {
        public override ushort Id
        {
            get { return 129; }
        }

        public override double Hardness
        {
            get { return 3; }
        }
    }
}
