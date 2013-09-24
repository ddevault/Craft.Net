using System;
using Craft.Net.Common;
using System.IO;

namespace Craft.Net.Client
{
    public static class DotMinecraft
    {
        public static string GetDotMinecraftPath()
        {
            if (RuntimeInfo.IsLinux)
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".minecraft");
            if (RuntimeInfo.IsMacOSX)
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Library", "Application Support", ".minecraft");
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft");
        }
    }
}

