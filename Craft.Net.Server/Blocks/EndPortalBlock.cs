using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// End Portal ID=119
    /// </summary>
    /// <remarks></remarks>
    public class EndPortalBlock : Block
    {
        /// <summary>
        /// The Block ID for this block
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 119; }
        }

        /// <summary>
        /// Returns the opacity of a block.
        /// EndPortalBlock is a NonSolid.
        /// </summary>
        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.NonSolid; }
        }
    }
}