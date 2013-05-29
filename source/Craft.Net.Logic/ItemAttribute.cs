using Craft.Net.Logic.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Logic
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ItemAttribute : Attribute
    {
        public short ItemId { get; set; }
        public string Initializer { get; set; }
        public string DisplayName { get; set; }
        public Type InitializerType { get; set; }

        public ItemAttribute(short itemId, string displayName = null,
            string initializer = null, Type initializerType = null)
        {
            ItemId = itemId;
            DisplayName = displayName;
            Initializer = initializer;
            InitializerType = initializerType;
        }
    }
}
