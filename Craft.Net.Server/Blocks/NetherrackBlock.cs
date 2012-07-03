using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Netherrack block (ID = 87)
    /// </summary>
    /// <remarks></remarks>
    public class NetherrackBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (87)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 87; }
        }
    }
}
