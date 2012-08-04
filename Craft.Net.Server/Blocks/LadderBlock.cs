using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Blocks
{
    public enum LadderFacing
    {
        North = 2,
        South = 3,
        West = 4,
        East = 5
    }

    /// <summary>
    /// A Ladder block (ID = 65)
    /// </summary>
    /// <remarks></remarks>
    public class LadderBlock : Block
    {
        public LadderBlock()
        {
        }

        public LadderBlock(LadderFacing Facing)
        {
            this.Metadata = (byte)Facing;
        }

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
