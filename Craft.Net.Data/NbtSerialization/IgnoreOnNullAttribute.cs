using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.NbtSerialization
{
   [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
   public sealed class IgnoreOnNullAttribute : Attribute { }
}