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
        public double Hardness { get; set; }

        public BlockAttribute(short BlockId, string DisplayName = null,  string Initializer = null,
            double Hardness = 0)
        {
            this.BlockId = BlockId;
            this.DisplayName = DisplayName;
            this.Initializer = Initializer;
        }
    }
}
