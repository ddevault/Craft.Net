using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
    public class SugarCaneBlock : Block
    {
        public override ushort Id
        {
            get { return 83; }
        }

        public override BoundingBox? BoundingBox
        {
            get { return null; }
        }

        public override bool GetDrop(ToolItem tool, out Slot[] drop)
        {
            drop = new[] { new Slot((ushort)new SugarCanesItem(), 1) };
            return true;
        }
    }
}