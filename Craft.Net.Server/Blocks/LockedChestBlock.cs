using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Locked Chest block (ID = 95)
    /// </summary>
    /// <remarks></remarks>
    public class LockedChestBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (ID = 95)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 95; }
        }
    }
}
