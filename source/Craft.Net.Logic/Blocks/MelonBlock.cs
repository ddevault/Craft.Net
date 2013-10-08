using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(MelonBlock.BlockId, MelonBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(MelonBlock.BlockId, DisplayName = MelonBlock.DisplayName, Hardness = MelonBlock.Hardness)]
    public static class MelonBlock
    {
        public const string DisplayName = "Melon";
        public const short BlockId = 103;
        public const double Hardness = 1;
    }
}
