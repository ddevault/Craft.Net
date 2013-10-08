using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(ButtonBlock.BlockId, ButtonBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(ButtonBlock.BlockId, DisplayName = ButtonBlock.DisplayName, Hardness = ButtonBlock.Hardness)]
    public static class ButtonBlock
    {
        public const string DisplayName = "Button";
        public const short BlockId = 77;
        public const double Hardness = 0.5;
    }
}
