using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
    public class EmeraldBlock : Block
    {
        public override short Id
        {
            get { return 133; }
        }

        public override double Hardness
        {
            get { return 5; }
        }

        public override bool CanHarvest(Items.ToolItem tool)
        {
            return tool is PickaxeItem &&
                (tool.ToolMaterial == ToolMaterial.Iron ||
                tool.ToolMaterial == ToolMaterial.Gold ||
                tool.ToolMaterial == ToolMaterial.Diamond);
        }
    }
}
