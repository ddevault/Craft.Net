using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(GlassPaneBlock.BlockId, GlassPaneBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(GlassPaneBlock.BlockId, DisplayName = GlassPaneBlock.DisplayName, Hardness = GlassPaneBlock.Hardness)]
    public static class GlassPaneBlock
    {
        public const string DisplayName = "Glass Pane";
        public const short BlockId = 102;
        public const double Hardness = 0.3;
    }
}
