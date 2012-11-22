using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.NbtSerialization
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class)]
    public class TagNameAttribute : Attribute
    {
        public string Name { get; set; }

        /// <summary>
        /// Decorates the given property or field with the specified
        /// NBT tag name.
        /// </summary>
        public TagNameAttribute(string name)
        {
            Name = name;
        }
    }
}