using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
    public class CoalOreBlock : Block
    {
        public override short Id
        {
            get { return 16; }
        }

        public override double Hardness
        {
            get { return 3; }
        }

        public override bool CanHarvest(ToolItem tool)
        {
            return tool is PickaxeItem;
        }

        public override bool GetDrop(ToolItem tool, out ItemStack[] drop)
        {
            drop = new[] { new ItemStack(new CoalItem(), 1) };
            return CanHarvest(tool);
        }
    }
}
