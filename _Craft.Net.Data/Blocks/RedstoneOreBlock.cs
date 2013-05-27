using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Entities;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
    public class RedstoneOreBlock : Block
    {
        public override short Id
        {
            get { return 73; }
        }

        public override double Hardness
        {
            get { return 3; }
        }

        public override void OnBlockWalkedOn(World world, Vector3 position, Entity entity)
        {
            world.SetBlock(position, new RedstoneOreActiveBlock());
        }

        public override bool CanHarvest(ToolItem tool)
        {
            return tool is PickaxeItem &&
                (tool.ToolMaterial == ToolMaterial.Iron ||
                tool.ToolMaterial == ToolMaterial.Gold ||
                tool.ToolMaterial == ToolMaterial.Diamond);
        }

        public override bool GetDrop(ToolItem tool, out ItemStack[] drop)
        {
            drop = new[] { new ItemStack(new RedstoneItem(), (sbyte)MathHelper.Random.Next(4, 5)) };
            return tool is PickaxeItem &&
                (tool.ToolMaterial == ToolMaterial.Iron ||
                tool.ToolMaterial == ToolMaterial.Gold ||
                tool.ToolMaterial == ToolMaterial.Diamond);
        }
    }
}
