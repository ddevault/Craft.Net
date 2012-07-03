using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Red Mushroom block (ID = 40)
    /// </summary>
    /// <remarks></remarks>
    public class RedMushroomBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (40)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 40; }
        }

        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.Plant; }
        }
    }
}
