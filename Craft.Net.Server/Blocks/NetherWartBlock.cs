using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Nether Wart block (ID = 115)
    /// </summary>
    /// <remarks></remarks>
    public class NetherWartBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (115)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 115; }
        }
    }
}
