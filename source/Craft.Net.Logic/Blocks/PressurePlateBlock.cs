using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(PressurePlateBlock.BlockId, PressurePlateBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(PressurePlateBlock.BlockId, DisplayName = PressurePlateBlock.DisplayName, Hardness = PressurePlateBlock.Hardness)]
    public static class PressurePlateBlock
    {
        public const string DisplayName = "Pressure Plate";
        public const short BlockId = 70;
        public const double Hardness = 0.5;
    }
}
