using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
    public class DiamondOreBlock : Block
    {
        public override short Id
        {
            get { return 56; }
        }

		public override double Hardness 
        {
            get { return 3; }
        }

        public override bool CanHarvest(Items.ToolItem tool)
        {
            return tool is PickaxeItem &&
                (tool.ToolMaterial == ToolMaterial.Iron ||
                tool.ToolMaterial == ToolMaterial.Gold ||
                tool.ToolMaterial == ToolMaterial.Diamond);
        }

        public override bool GetDrop(ToolItem tool, out ItemStack[] drop)
        {
            drop = new[] { new ItemStack(new DiamondItem()) };
            return CanHarvest(tool);
        }
    }
}
