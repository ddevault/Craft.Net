using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [Item(SignItem.ItemId, SignItem.DisplayName)]
    public static class SignItem
    {
        public const short ItemId = 323;
        public const string DisplayName = "Sign";
    }
}
