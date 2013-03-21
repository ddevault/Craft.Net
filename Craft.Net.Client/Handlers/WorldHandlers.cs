using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Client.Events;
using Ionic.Zlib;
using Craft.Net.Data;
using System.IO;

namespace Craft.Net.Client.Handlers
{
    internal static class WorldHandlers
    {
        public static byte[] ChunkRemovalSequence = new byte[] { 0x78, 0x9C, 0x63, 0x64, 0x1C, 0xD9, 0x00, 0x00, 0x81, 0x80, 0x01, 0x01 };
        private const int BlockDataLength = Section.Width * Section.Depth * Section.Height;
        private const int NibbleDataLength = BlockDataLength / 2;

        public static void MapChunkBulk(MinecraftClient client, IPacket _packet)
        {
            var packet = (MapChunkBulkPacket)_packet;
            var metadataStrem = new MemoryStream(packet.ChunkMetadata);
            var minecraftStream = new MinecraftStream(metadataStrem);
            var data = ZlibStream.UncompressBuffer(packet.ChunkData);
            int chunkLength = BlockDataLength + (NibbleDataLength * 2) + (Chunk.Width * Chunk.Depth);
            if (packet.LightIncluded)
                chunkLength += NibbleDataLength;
            for (int i = 0; i < packet.ChunkCount; i++)
            {
                int x = minecraftStream.ReadInt32();
                int z = minecraftStream.ReadInt32();
                ushort primaryBitMap = minecraftStream.ReadUInt16();
                ushort addBitMap = minecraftStream.ReadUInt16(); // TODO
                int offset = i * chunkLength;
                // Read chunk data
                var chunk = new Chunk(World.GetRelativeChunkPosition(new Vector3(x, 0, z)));
                var sectionCount = 0;
                // Get the total sections included in the packet
                for (int y = 0; y < 16; y++)
                {
                    if ((primaryBitMap & (1 << y)) > 0)
                        sectionCount++;
                }
                // Run through the sections
                // TODO: Support block IDs >255
                for (int y = 0; y < 16; y++)
                {
                    if ((primaryBitMap & (1 << y)) > 0)
                    {
                        // Add this section
                        Array.Copy(data, offset + (y * BlockDataLength), chunk.Sections[y].Blocks, 0, BlockDataLength);
                        Array.Copy(data, offset + (y * BlockDataLength + (BlockDataLength * sectionCount)),
                            chunk.Sections[y].Metadata.Data, 0, NibbleDataLength);
                        Array.Copy(data, offset + (y * BlockDataLength + (BlockDataLength * sectionCount + NibbleDataLength)),
                            chunk.Sections[y].BlockLight.Data, 0, NibbleDataLength);
                        if (packet.LightIncluded)
                        {
                            Array.Copy(data, offset + (y * BlockDataLength + (BlockDataLength * sectionCount + (NibbleDataLength * 2))),
                                chunk.Sections[y].SkyLight.Data, 0, NibbleDataLength);
                        }
                    }
                }
                Array.Copy(data, offset + chunkLength - chunk.Biomes.Length, chunk.Biomes, 0, chunk.Biomes.Length);
                client.World.SetChunk(new Vector3(x, 0, z), chunk);
                client.OnChunkRecieved(new ChunkRecievedEventArgs(new Vector3(x, 0, z), new ReadOnlyChunk(chunk)));
            }
        }

        public static void ChunkData(MinecraftClient client, IPacket _packet)
        {
            var packet = (ChunkDataPacket)_packet;
            if (packet.Data == ChunkRemovalSequence)
            {
                client.World.RemoveChunk(packet.X, packet.Z);
                return;
            }
            var chunk = new Chunk(World.GetRelativeChunkPosition(new Vector3(packet.X, 0, packet.Z)));
            var data = ZlibStream.UncompressBuffer(packet.Data);
            var sectionCount = 0;
            // Get the total sections included in the packet
            for (int y = 0; y < 16; y++)
            {
                if ((packet.PrimaryBitMap & (1 << y)) > 0)
                    sectionCount++;
            }
            // Run through the sections
            // TODO: Support block IDs >255
            for (int y = 0; y < 16; y++)
            {
                if ((packet.PrimaryBitMap & (1 << y)) > 0)
                {
                    // Add this section
                    Array.Copy(data, y * BlockDataLength, chunk.Sections[y].Blocks, 0, BlockDataLength);
                    Array.Copy(data, y * BlockDataLength + (BlockDataLength * sectionCount),
                        chunk.Sections[y].Metadata.Data, 0, NibbleDataLength);
                    Array.Copy(data, y * BlockDataLength + (BlockDataLength * sectionCount + NibbleDataLength),
                        chunk.Sections[y].BlockLight.Data, 0, NibbleDataLength);
                    Array.Copy(data, y * BlockDataLength + (BlockDataLength * sectionCount + (NibbleDataLength * 2)),
                        chunk.Sections[y].SkyLight.Data, 0, NibbleDataLength);
                }
            }
            if (packet.GroundUpContinuous)
                Array.Copy(data, data.Length - chunk.Biomes.Length, chunk.Biomes, 0, chunk.Biomes.Length);
            client.World.SetChunk(new Vector3(packet.X, 0, packet.Z), chunk);
            client.OnChunkRecieved(new ChunkRecievedEventArgs(new Vector3(packet.X, 0, packet.Z), new ReadOnlyChunk(chunk)));
        }
    }
}
