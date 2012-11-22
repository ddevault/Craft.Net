using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
    public class RedstoneLampActiveBlock : RedstoneLampBlock
    {
        public override ushort Id
        {
            get { return 124; }
        }

        public override bool GetDrop(ToolItem tool, out Slot[] drop)
        {
            drop = new[] { new Slot((ushort)new RedstoneLampBlock(), 1) };
            return true;
        }
    }
}