using Craft.Net.Anvil;
using Craft.Net.Common;
using Craft.Net.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server
{
    public static class LogicHelpers
    {
        private static bool Registered = false;

        public static void Register()
        {
            if (Registered)
                return;
            Registered = true;
        }
    }
}
