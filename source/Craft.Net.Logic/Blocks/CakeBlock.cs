using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Logic.Blocks
{
    [Item(CakeBlock.BlockId, CakeBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(CakeBlock.BlockId, CakeBlock.DisplayName)]
    public static class CakeBlock
    {
        public const string DisplayName = "Cake";
        public const short BlockId = 92;
    }
}
