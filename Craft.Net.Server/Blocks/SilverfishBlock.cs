using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Silverfish block (ID = 97)
    /// This block may look like a Stone, Cobblestone or a Stone Brick block but when destoryed will spawn a silverfish
    /// </summary>
    /// <remarks></remarks>
    public class SilverfishBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (97)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 97; }
        }
    }
}
