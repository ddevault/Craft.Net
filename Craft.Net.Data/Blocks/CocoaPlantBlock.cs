using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
    public class CocoaPlantBlock : Block
    {
        public override ushort Id
        {
            get { return 127; }
        }

        public override double Hardness
        {
            get { return 0.2; }
        }

        public override BoundingBox? BoundingBox
        {
            get { return null; }
        }

        public override bool GetDrop(ToolItem tool, out Slot[] drop)
        {
            if ((Metadata & 0xC) <= 4)
                drop = new[] { new Slot((ushort)new CocoaBeanItem(), 1) };
            else
                drop = new[] { new Slot((ushort)new CocoaBeanItem(), 3) };
            return true;
        }

        public override bool RequiresSupport
        {
            get { return true; }
        }

        public override Vector3 SupportDirection
        {
            get { return Vector3.East; } // TODO
        }
    }
}