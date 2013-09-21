using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(GlowstoneBlock.BlockId, GlowstoneBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(GlowstoneBlock.BlockId, DisplayName = GlowstoneBlock.DisplayName, Hardness = GlowstoneBlock.Hardness)]
    public static class GlowstoneBlock
    {
        public const string DisplayName = "Glowstone";
        public const short BlockId = 89;
        public const double Hardness = 0.3;
    }
}
