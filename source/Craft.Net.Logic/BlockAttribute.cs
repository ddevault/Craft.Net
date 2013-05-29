using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Logic
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BlockAttribute : Attribute
    {
        public short BlockId { get; set; }
        public string DisplayName { get; set; }
        public string Initializer { get; set; }

        public BlockAttribute(short blockId, string displayName = null, string initializer = null)
        {
            BlockId = blockId;
            DisplayName = displayName;
            Initializer = initializer;
        }
    }
}
