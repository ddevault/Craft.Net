using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(GlassBlock.BlockId, GlassBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(GlassBlock.BlockId, DisplayName = GlassBlock.DisplayName, Hardness = GlassBlock.Hardness)]
    public static class GlassBlock
    {
        public const string DisplayName = "Glass";
        public const short BlockId = 20;
        public const double Hardness = 0.3;
    }
}
