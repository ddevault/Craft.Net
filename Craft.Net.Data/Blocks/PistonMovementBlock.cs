using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class PistonMovementBlock : Block
    {
        public override ushort Id
        {
            get { return 36; }
        }

        public override double Hardness
        {
            get { return 0.5; } // TODO: This is a guess
        }
    }
}