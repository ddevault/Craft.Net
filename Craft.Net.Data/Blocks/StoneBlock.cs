using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
    public class StoneBlock : Block
    {
        public override short Id
        {
            get { return 1; }
        }

        public override double Hardness
        {
            get { return 1; }
        }

        public override bool CanHarvest(ToolItem tool)
        {
            return tool is PickaxeItem;
        }

        public override bool GetDrop(ToolItem tool, out ItemStack[] drop)
        {
            drop = new[] { new ItemStack(new CobblestoneBlock(), 1) };
            return CanHarvest(tool);
        }
    }
}
