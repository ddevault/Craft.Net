using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
    public class RedstoneOreActiveBlock : RedstoneOreBlock
    {
        // TODO: Schedule update to deactivate
        public override short Id
        {
            get { return 74; }
        }
    }
}
