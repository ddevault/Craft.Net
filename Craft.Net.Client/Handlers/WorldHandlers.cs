using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Client.Handlers
{
    internal static class WorldHandlers
    {
        public static byte[] ChunkRemovalSequence = new byte[] { 0x78, 0x9C, 0x63, 0x64, 0x1C, 0xD9, 0x00, 0x00, 0x81, 0x80, 0x01, 0x01 };

        public static void MapChunkBulk(MinecraftClient client, IPacket _packet)
        {
            var packet = (MapChunkBulkPacket)_packet;

        }

        public static void ChunkData(MinecraftClient client, IPacket _packet)
        {
            var packet = (ChunkDataPacket)_packet;
            if (packet.Data == ChunkRemovalSequence)
            {
                client.World.RemoveChunk(packet.X, packet.Z);
                return;
            }
        }
    }
}
