using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Items
{
    public abstract class SwordItem : ToolItem
    {
        public override ToolType ToolType
        {
            get { return ToolType.Sword; }
        }
    }
}
