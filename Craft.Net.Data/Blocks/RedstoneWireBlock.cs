using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class RedstoneWireBlock : Block
    {
        public override ushort Id
        {
            get { return 55; }
        }

        public override BoundingBox? BoundingBox
        {
            get { return null; }
        }
    }
}
