using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Anvil;

namespace Craft.Net.Logic.Blocks
{
    [Item(BedrockBlock.BlockId, BedrockBlock.DisplayName, "Initialize", typeof(Block))]
    [Block(BedrockBlock.BlockId, DisplayName = BedrockBlock.DisplayName, Hardness = BedrockBlock.Hardness)]
    public static class BedrockBlock
    {
        public const string DisplayName = "Bedrock";
        public const short BlockId = 7;
        public const double Hardness = 0;
    }
}
