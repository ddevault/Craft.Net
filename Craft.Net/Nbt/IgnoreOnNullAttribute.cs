using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Nbt
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class IgnoreOnNullAttribute : Attribute { }
}
