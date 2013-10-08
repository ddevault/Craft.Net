using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(MushroomBlock.BlockId, MushroomBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(MushroomBlock.BlockId, DisplayName = MushroomBlock.DisplayName, Hardness = MushroomBlock.Hardness)]
    public static class MushroomBlock
    {
        public const string DisplayName = "Mushroom";
        public const short BlockId = 39;
        public const double Hardness = 0;
    }
}
