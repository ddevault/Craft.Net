using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(WoodenPlanksBlock.BlockId, WoodenPlanksBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(WoodenPlanksBlock.BlockId, DisplayName = WoodenPlanksBlock.DisplayName, Hardness = WoodenPlanksBlock.Hardness)]
    public static class WoodenPlanksBlock
    {
        public const string DisplayName = "Wooden Planks";
        public const short BlockId = 5;
        public const double Hardness = 2;
    }
}
