using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Flowing Lava block (ID = 10)
    /// </summary>
    /// <remarks></remarks>
    public class LavaFlowingBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (10)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 10; }
        }

        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.Fluid; }
        }
    }
}
