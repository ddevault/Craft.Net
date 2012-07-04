using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Items
{
    /// <summary>
    /// A loaf of Bread (ID = 297)
    /// </summary>
    /// <remarks></remarks>
    public class BreadItem : ConsumableItem
    {
        /// <summary>
        /// The ID for this Item (297) 
        /// </summary>
        /// <remarks></remarks>
        public override short ItemID
        {
            get { return 297; }
        }
    }
}
