using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// An Obsidian block (ID = 49)
    /// </summary>
    /// <remarks></remarks>
    public class ObsidianBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (49)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 49; }
        }
    }
}
