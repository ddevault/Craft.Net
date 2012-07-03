using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Sign block on a Wall (ID = 68)
    /// </summary>
    /// <remarks></remarks>
    public class WallSignBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (68)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 68; }
        }
    }
}
