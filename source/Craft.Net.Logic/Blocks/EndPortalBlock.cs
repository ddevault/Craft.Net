using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(EndPortalBlock.BlockId, EndPortalBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(EndPortalBlock.BlockId, DisplayName = EndPortalBlock.DisplayName, Hardness = EndPortalBlock.Hardness)]
    public static class EndPortalBlock
    {
        public const string DisplayName = "End Portal";
        public const short BlockId = 120;
        public const double Hardness = -1;
    }
}
