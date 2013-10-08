using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(BlockofDiamondBlock.BlockId, BlockofDiamondBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(BlockofDiamondBlock.BlockId, DisplayName = BlockofDiamondBlock.DisplayName, Hardness = BlockofDiamondBlock.Hardness)]
    public static class BlockofDiamondBlock
    {
        public const string DisplayName = "Block of Diamond";
        public const short BlockId = 57;
        public const double Hardness = 5;
    }
}
