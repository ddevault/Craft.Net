using Craft.Net.Logic.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Logic
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MinecraftItemAttribute : Attribute
    {
        public short ItemId { get; set; }
        public string Initializer { get; set; }
        public string DisplayName { get; set; }

        public MinecraftItemAttribute(short itemId, string displayName = null, string initializer = null)
        {
            ItemId = itemId;
            DisplayName = displayName;
            Initializer = initializer;
        }
    }
}
