using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class BookshelfBlock : Block
    {
        public override ushort Id
        {
            get { return 47; }
        }

        public override double Hardness
        {
            get { return 1.5; }
        }
    }
}
