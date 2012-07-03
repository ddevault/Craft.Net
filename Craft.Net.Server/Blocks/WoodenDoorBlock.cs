using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Wooden Door block (ID = 64)
    /// </summary>
    /// <remarks></remarks>
    public class WoodenDoorBlock : DoorBlock
    {
        /// <summary>
        /// The Block ID for this block (64)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 64; }
        }
    }
}
