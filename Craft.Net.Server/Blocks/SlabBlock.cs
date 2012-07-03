using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A full slab (43)
    /// </summary>
    public class SlabBlock : Block
    {
        /// <summary>
        /// The Block ID for this block
        /// </summary>
        public override byte BlockID
        {
            get { return 43; }
        }
    }
}

