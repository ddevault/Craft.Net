using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(CobbleWallBlock.BlockId, CobbleWallBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(CobbleWallBlock.BlockId, DisplayName = CobbleWallBlock.DisplayName, Hardness = CobbleWallBlock.Hardness)]
    public static class CobbleWallBlock
    {
        public const string DisplayName = "CobbleWall";
        public const short BlockId = 139;
        public const double Hardness = 0;
    }
}
