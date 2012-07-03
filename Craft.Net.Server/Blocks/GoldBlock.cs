using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Gold block (ID = 41)
    /// </summary>
    /// <remarks></remarks>
    public class GoldBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (41)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 41; }
        }
    }
}

