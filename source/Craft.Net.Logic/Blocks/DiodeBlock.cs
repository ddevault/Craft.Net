using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(DiodeBlock.BlockId, DiodeBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(DiodeBlock.BlockId, DisplayName = DiodeBlock.DisplayName, Hardness = DiodeBlock.Hardness)]
    public static class DiodeBlock
    {
        public const string DisplayName = "Diode";
        public const short BlockId = 93;
        public const double Hardness = 0;
    }
}
