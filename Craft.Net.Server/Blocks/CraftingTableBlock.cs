using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Crafting Table Block (ID = 58)
    /// </summary>
    /// <remarks></remarks>
    public class CraftingTableBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (58)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 58; }
        }
    }
}

