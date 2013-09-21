using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(LapisLazuliBlockBlock.BlockId, LapisLazuliBlockBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(LapisLazuliBlockBlock.BlockId, DisplayName = LapisLazuliBlockBlock.DisplayName, Hardness = LapisLazuliBlockBlock.Hardness)]
    public static class LapisLazuliBlockBlock
    {
        public const string DisplayName = "Lapis Lazuli Block";
        public const short BlockId = 22;
        public const double Hardness = 3;
    }
}
