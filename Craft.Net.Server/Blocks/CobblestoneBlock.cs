using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Cobblestone Block (ID = 4)
    /// </summary>
    /// <remarks></remarks>
    public class CobblestoneBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (4)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 4; }
        }
    }
}
