using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class MobSpawnerBlock : Block
    {
        public override short Id
        {
            get { return 52; }
        }

        public override double Hardness
        {
            get { return 5; }
        }

        public override bool CanHarvest(Items.ToolItem tool)
        {
            return false;
        }
    }
}
