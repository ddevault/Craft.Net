using System;

namespace Craft.Net.Server.Blocks
{
    public class BedrockBlock : Block
    {
        public BedrockBlock()
        {
        }

        public override byte BlockId
        {
            get
            {
                return 7;
            }
        }
    }
}

