using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Lilypad block (ID = 111)
    /// </summary>
    /// <remarks></remarks>
    public class LilyPadBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (111)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 111; }
        }
    }
}
