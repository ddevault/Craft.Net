using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// An Iron Door block (ID = 71)
    /// </summary>
    /// <remarks></remarks>
    public class IronDoorBlock : DoorBlock
    {
        /// <summary>
        /// The Block ID for this block (71)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 71; }
        }
    }
}
