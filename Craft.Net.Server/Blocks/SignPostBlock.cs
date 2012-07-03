using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// Class of the Sign Post Block (ID = 63)
    /// </summary>
    public class SignPostBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (63)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 63; }
        }

        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.NonSolid; }
        }
    }
}
