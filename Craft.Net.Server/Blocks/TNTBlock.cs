using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A TNT block (ID = 46)
    /// </summary>
    /// <remarks></remarks>
    public class TNTBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (46)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 46; }
        }


        /// <summary>
        /// TNT is a cube solid
        /// </summary>
        public override BlockOpacity Transparent
        {
            get
            {
                return BlockOpacity.CubeSolid;
            }
        }
    }
}
