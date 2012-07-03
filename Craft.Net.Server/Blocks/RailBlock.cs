using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Minecart Rail block (ID = 66)
    /// </summary>
    /// <remarks></remarks>
    public class RailBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (66)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 66; }
        }

        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.NonSolidMechanism; }
        }
    }
}
