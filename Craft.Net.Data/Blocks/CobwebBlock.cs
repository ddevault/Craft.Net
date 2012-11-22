using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
    public class CobwebBlock : Block
    {
        public override ushort Id
        {
            get { return 30; }
        }

        public override double Hardness
        {
            get { return 4; }
        }

        public override bool CanHarvest(ToolItem tool)
        {
            return false;
        }
    }
}