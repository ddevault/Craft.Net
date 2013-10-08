using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(NetherQuartzOreBlock.BlockId, NetherQuartzOreBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(NetherQuartzOreBlock.BlockId, DisplayName = NetherQuartzOreBlock.DisplayName, Hardness = NetherQuartzOreBlock.Hardness)]
    public static class NetherQuartzOreBlock
    {
        public const string DisplayName = "Nether Quartz Ore";
        public const short BlockId = 153;
        public const double Hardness = 3;
    }
}
