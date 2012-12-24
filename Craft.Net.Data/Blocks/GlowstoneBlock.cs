using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
    public class GlowstoneBlock : Block
    {
        public override short Id
        {
            get { return 89; }
        }

        public override double Hardness
        {
            get { return 0.3; }
        }

        public override bool GetDrop(ToolItem tool, out ItemStack[] drop)
        {
            drop = new[] { new ItemStack(new GlowstoneDustItem(), (sbyte)MathHelper.Random.Next(2, 4)) };
            return true;
        }
    }
}
