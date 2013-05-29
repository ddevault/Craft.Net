using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Items
{
    [Item(PaperItem.ItemId, PaperItem.DisplayName)]
    public static class PaperItem
    {
        public const short ItemId = 339;
        public const string DisplayName = "Paper";
    }
}
