using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class WoodenButtonBlock : StoneButtonBlock // TODO: Generic ButtonBlock
    {
        public override ushort Id
        {
            get { return 143; }
        }

        public override BoundingBox? BoundingBox
        {
            get { return null; }
        }
    }
}