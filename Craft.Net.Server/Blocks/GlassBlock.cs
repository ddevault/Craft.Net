using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Glass block (ID = 20)
    /// </summary>
    /// <remarks></remarks>
    public class GlassBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (20)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 20; }
        }


        /// <summary>
        /// Glass is a cube solid
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
