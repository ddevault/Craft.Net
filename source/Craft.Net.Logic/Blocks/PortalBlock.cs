using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(PortalBlock.BlockId, PortalBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(PortalBlock.BlockId, DisplayName = PortalBlock.DisplayName, Hardness = PortalBlock.Hardness)]
    public static class PortalBlock
    {
        public const string DisplayName = "Portal";
        public const short BlockId = 90;
		public const double Hardness = -1;
    }
}
