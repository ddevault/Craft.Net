using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
    public class TripwireBlock : Block
    {
        public override short Id
        {
            get { return 132; }
        }

        public override BoundingBox? BoundingBox
        {
            get { return null; }
        }

        public override bool GetDrop(ToolItem tool, out ItemStack[] drop)
        {
            drop = new[] { new ItemStack(new StringItem(), 1) };
            return true;
        }
    }
}
