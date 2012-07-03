using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Stone Pressure Plate block (ID = 70)
    /// </summary>
    /// <remarks></remarks>
    public class StonePressurePlateBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (70)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 70; }
        }

        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.NonSolidMechanism; }
        }
    }
}
