using System;

namespace Craft.Net.Logic
{
    public struct BlockInfo
    {
        public BlockInfo(short blockId, byte metadata, byte skyLight, byte blockLight)
        {
            BlockId = blockId;
            Metadata = metadata;
            SkyLight = skyLight;
            BlockLight = blockLight;
        }

        public short BlockId;
        public byte Metadata;
        public byte SkyLight;
        public byte BlockLight;
    }
}