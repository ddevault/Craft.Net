using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Items
{
    /// <summary>
    /// A Book item (ID = 340)
    /// </summary>
    /// <remarks></remarks>
    public class BookItem : Item
    {
        /// <summary>
        /// The ID for this Item (340)
        /// </summary>
        /// <remarks></remarks>
        public override short ItemID
        {
            get { return 340; }
        }
    }
}
