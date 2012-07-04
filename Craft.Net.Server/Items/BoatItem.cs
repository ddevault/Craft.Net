using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Items
{
    /// <summary>
    /// A Boat item (ID = 333)
    /// </summary>
    /// <remarks></remarks>
    public class BoatItem : Item
    {
        /// <summary>
        /// The ID for this Item (333)
        /// </summary>
        /// <remarks></remarks>
        public override short ItemID
        {
            get { return 333; }
        }
    }
}
