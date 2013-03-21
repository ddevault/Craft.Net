using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Client.Handlers
{
    internal static class WorldHandlers
    {
        public static void MapChunkBulk(MinecraftClient client, IPacket _packet)
        {
            var packet = (MapChunkBulkPacket)_packet;

        }

        public static void ChunkData(MinecraftClient client, IPacket _packet)
        {
            var packet = (ChunkDataPacket)_packet;
        }
    }
}
