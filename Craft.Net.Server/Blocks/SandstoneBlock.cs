using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Sandstone block (ID = 24)
    /// </summary>
    /// <remarks></remarks>
    public class SandstoneBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (24)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 24; }
        }
    }
}

