using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(QuartzBlockBlock.BlockId, QuartzBlockBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(QuartzBlockBlock.BlockId, DisplayName = QuartzBlockBlock.DisplayName, Hardness = QuartzBlockBlock.Hardness)]
    public static class QuartzBlockBlock
    {
        public const string DisplayName = "QuartzBlock";
        public const short BlockId = 155;
        public const double Hardness = 0.8;
    }
}
