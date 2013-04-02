using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class TintedGlassBlock : Block
    {
        public WoolColor Color
        {
            get { return (WoolColor)Metadata; }
            set { Metadata = (byte)value; }
        }

        public override short Id
        {
            get { return 163; }
        }
    }
}
