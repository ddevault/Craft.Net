using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
    public class ClayBlock : Block
    {
        public override short Id
        {
            get { return 82; }
        }

        public override double Hardness
        {
            get { return 0.6; }
        }

        public override bool GetDrop(Items.ToolItem tool, out ItemStack[] drop)
        {
            drop = new[] { new ItemStack(new ClayItem(), 4) };
            return true;
        }
    }
}
