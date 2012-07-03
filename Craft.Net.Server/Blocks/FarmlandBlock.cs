using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// Farmland Block ID = 60
    /// </summary>
    /// <remarks></remarks>
    public class FarmlandBlock : Block
    {
        /// <summary>
        /// The Block ID for this block
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 60; }
        } // TODO: PlayerWalkedOn

        /// <summary>
        /// Returns the opacity of a block.
        /// FarmlandBlock is a NonCubeSolid.
        /// </summary>
        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.NonCubeSolid; }
        }

    }
}
