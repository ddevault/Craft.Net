using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Wooden Pressure Plate block (ID = 72)
    /// </summary>
    /// <remarks></remarks>
    public class WoodPressurePlateBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (72)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 72; }
        }

        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.NonSolidMechanism; }
        }
    }
}
