using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
    public class NetherQuartzOreBlock : Block
    {
        public override short Id
        {
            get { return 153; }
        }

        public override bool GetDrop(ToolItem tool, out ItemStack[] drop)
        {
            drop = new[] { new ItemStack(new NetherQuartzItem()) };
            return tool is PickaxeItem;
        }
    }
}
