using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(SignBlock.BlockId, SignBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(SignBlock.BlockId, DisplayName = SignBlock.DisplayName, Hardness = SignBlock.Hardness)]
    public static class SignBlock
    {
        public const string DisplayName = "Sign";
        public const short BlockId = 63;
        public const double Hardness = 1;
    }
}
