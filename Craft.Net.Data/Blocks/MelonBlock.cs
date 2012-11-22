using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
    public class MelonBlock : Block
    {
        public override ushort Id
        {
            get { return 103; }
        }

        public override double Hardness
        {
            get { return 1; }
        }

        public override bool GetDrop(ToolItem tool, out Slot[] drop)
        {
            drop = new[] { new Slot((ushort)new MelonItem(), (byte)DataUtility.Random.Next(3, 7)) };
            return true;
        }
    }
}