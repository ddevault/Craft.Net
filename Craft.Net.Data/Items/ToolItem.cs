using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Items
{
    public abstract class ToolItem : Item
    {
        public override byte MaximumStack
        {
            get { return 1; }
        }
    }
}
