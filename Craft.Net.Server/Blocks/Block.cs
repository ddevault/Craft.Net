using System;

namespace Craft.Net.Server.Blocks
{
    public abstract class Block
    {
        public byte Metadata, BlockLight, SkyLight;

        public abstract byte BlockId
        {
            get;
        }

        public Block()
        {
        }
    }
}

