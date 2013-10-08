using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(EmeraldOreBlock.BlockId, EmeraldOreBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(EmeraldOreBlock.BlockId, DisplayName = EmeraldOreBlock.DisplayName, Hardness = EmeraldOreBlock.Hardness)]
    public static class EmeraldOreBlock
    {
        public const string DisplayName = "Emerald Ore";
        public const short BlockId = 129;
        public const double Hardness = 3;
    }
}
