using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public enum SkullDirection
    {
        Floor = 1,
    }

    public class SkullBlock : Block // TODO: Tile entity
    {
        public override ushort Id
        {
            get { return 144; }
        }
    }
}
