using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Nether Portal block (ID = 90)
    /// </summary>
    /// <remarks></remarks>
    public class NetherPortalBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (90)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 90; }
        }
        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.NonSolid; }
        }
    }
}
