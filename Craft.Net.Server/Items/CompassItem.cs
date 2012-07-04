using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Items
{
    /// <summary>
    /// A Compass item (ID = 345)
    /// </summary>
    /// <remarks></remarks>
    public class CompassItem : Item
    {
        /// <summary>
        /// The ID for this Item (345)
        /// </summary>
        /// <remarks></remarks>
        public override short ItemID
        {
            get { return 345; }
        }
    }
}
