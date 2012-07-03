using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Lapis Lazuli block (ID = 22) (The block, not the ore)
    /// </summary>
    /// <remarks></remarks>
    public class LapisLazuliBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (22)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 22; }
        }
    }
}
