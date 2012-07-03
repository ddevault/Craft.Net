using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Ladder block (ID = 65)
    /// </summary>
    /// <remarks></remarks>
    public class LadderBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (65)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 65; }
        }

        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.NonCubeSolid; }
        }
    }
}
