using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// Leaf Block ID=18
    /// </summary>
    public class LeafBlockBlock:Block
    {
        /// <summary>
        /// The Block ID for this block (18)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 18; }
        }

        /// <summary>
        /// Leaves are cube solids.
        /// NOTE: Only true if on fancy graphics.
        /// </summary>
        public override BlockOpacity Transparent
        {
            get
            {
                return BlockOpacity.CubeSolid;
            }
        }
    }
}
