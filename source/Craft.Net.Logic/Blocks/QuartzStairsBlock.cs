using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(QuartzStairsBlock.BlockId, QuartzStairsBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(QuartzStairsBlock.BlockId, DisplayName = QuartzStairsBlock.DisplayName, Hardness = QuartzStairsBlock.Hardness)]
    public static class QuartzStairsBlock
    {
        public const string DisplayName = "Quartz Stairs";
        public const short BlockId = 156;
        public const double Hardness = 0;
    }
}
