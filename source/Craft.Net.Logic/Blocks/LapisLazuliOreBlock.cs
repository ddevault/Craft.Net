using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(LapisLazuliOreBlock.BlockId, LapisLazuliOreBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(LapisLazuliOreBlock.BlockId, DisplayName = LapisLazuliOreBlock.DisplayName, Hardness = LapisLazuliOreBlock.Hardness)]
    public static class LapisLazuliOreBlock
    {
        public const string DisplayName = "Lapis Lazuli Ore";
        public const short BlockId = 21;
        public const double Hardness = 3;
    }
}
