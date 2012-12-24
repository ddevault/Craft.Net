using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
    public class PotatoBlock : Block
    {
        public override short Id
        {
            get { return 142; }
        }

        public override BoundingBox? BoundingBox
        {
            get { return null; }
        }

        public override bool RequiresSupport
        {
            get { return true; }
        }

        public override Vector3 SupportDirection
        {
            get { return Vector3.Down; }
        }

        public override bool GetDrop(ToolItem tool, out ItemStack[] drop)
        {
            if (MathHelper.Random.Next(100) == 0)
            {
                drop = new[] { new ItemStack(new PotatoItem(), (sbyte)MathHelper.Random.Next(1, 4)),
                    new ItemStack(new PoisonousPotatoItem(), 1) };
            }
            else
            {
                drop = new[] { new ItemStack(new PotatoItem(), (sbyte)MathHelper.Random.Next(1, 4)) };
            }
            return true;
        }
    }
}
