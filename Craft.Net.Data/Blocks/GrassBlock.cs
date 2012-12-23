using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
    public class GrassBlock : Block
    {
        public override short Id
        {
            get { return 2; }
        }

        public override double Hardness
        {
            get { return 0.6; }
        }

        public override bool GetDrop(ToolItem tool, out Slot[] drop)
        {
            drop = new[] { new Slot(new DirtBlock(), 1) };
            return true;
        }
    }
}
