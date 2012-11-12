using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class GrassBlock : Block
    {
        public override ushort Id
        {
            get { return 2; }
        }

        public override double Hardness
        {
            get { return 0.6; }
        }

        public override Slot GetDrop()
        {
            return new Slot((ushort)new DirtBlock(), 1);
        }
    }
}
