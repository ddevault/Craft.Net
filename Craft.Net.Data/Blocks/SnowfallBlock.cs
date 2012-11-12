using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Entities;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
    public class SnowfallBlock : Block
    {
        public override ushort Id
        {
            get { return 78; }
        }

        public override double Hardness
        {
            get { return 0.1; }
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

        public override bool GetDrop(ToolItem tool, out Slot[] drop)
        {
            drop = new Slot[0];
            if (tool is ShovelItem)
                drop = new[] { new Slot((ushort)new SnowballItem(), 1) };
            return tool is ShovelItem;
        }
    }
}
