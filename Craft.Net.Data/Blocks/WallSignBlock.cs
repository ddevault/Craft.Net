using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class WallSignBlock : SignBlock
    {
        public WallSignBlock()
        {
        }

        public WallSignBlock(Direction direction)
        {
            Metadata = (byte)direction;
        }

        public override ushort Id
        {
            get { return 68; }
        }
    }
}
