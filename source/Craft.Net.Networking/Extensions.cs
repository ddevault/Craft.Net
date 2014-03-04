using System;
using Craft.Net.Networking;

namespace Craft.Net
{
    public static class Extensions
    {
        public static string ToJavaUUID(this Guid guid)
        {
            var result = string.Empty;
            foreach (var b in guid.ToByteArray())
                result += b.ToString("x2");
            return result;
        }
    }
}

