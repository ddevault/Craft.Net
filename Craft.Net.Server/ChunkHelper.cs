using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data;

namespace Craft.Net.Server
{
    public static class ChunkHelper
    {
        public static byte[] ChunkRemovalSequence = new byte[] { 0x78, 0x9C, 0x63, 0x64, 0x1C, 0xD9, 0x00, 0x00, 0x81, 0x80, 0x01, 0x01 };

        public static ChunkDataPacket CreatePacket(Chunk chunk)
        {
            return new ChunkDataPacket(); // TODO
        }
    }
}
