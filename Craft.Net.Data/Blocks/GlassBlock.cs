using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class GlassBlock : Block
    {
        public override short Id
        {
            get { return 20; }
        }

        public override double Hardness
        {
            get { return 0.3; }
        }

        public override byte LightReduction
        {
            get { return 0; }
        }

        public override bool CanHarvest(Items.ToolItem tool)
        {
            return false;
        }
    }
}
