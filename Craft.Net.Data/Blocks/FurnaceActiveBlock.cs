using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class FurnaceActiveBlock : FurnaceBlock
    {
        public override short Id
        {
            get { return 62; }
        }

        public override bool GetDrop(Items.ToolItem tool, out ItemStack[] drop)
        {
            drop = new[] { new ItemStack(new FurnaceBlock(), 1) };
            return true;
        }
    }
}
