using System;
using Craft.Net.Client.Events;
using Ionic.Zlib;
using Craft.Net.Anvil;
using Craft.Net.Networking;
using Craft.Net.Common;

namespace Craft.Net.Client.Handlers
{
    internal static class WorldHandlers
    {
        private const int BlockDataLength = Section.Width * Section.Depth * Section.Height;
        private const int NibbleDataLength = BlockDataLength / 2;

        private static int GetSectionCount(ushort bitMap)
        {
            // Get the total sections included in the bitMap
            var sectionCount = 0;
            for (int y = 0; y < 16; y++)
            {
                if ((bitMap & (1 << y)) > 0)
                    sectionCount++;
            }

            return sectionCount;
        }

        private static void AddChunk(MinecraftClient client, int x, int z, ushort primaryBitMap, ushort addBitMap, bool lightIncluded, bool groundUp, byte[] data)
        {
            var coordinates = new Coordinates2D(x, z);
            var relativePosition = GetRelativeChunkPosition(coordinates);
            var chunk = new Chunk(relativePosition);
            var sectionCount = GetSectionCount(primaryBitMap);

            // Run through the sections
            // TODO: Support block IDs >255
            for (int y = 0; y < 16; y++)
            {
                if ((primaryBitMap & (1 << y)) > 0)
                {
                    // Blocks
                    Array.Copy(data, y * BlockDataLength, chunk.Sections[y].Blocks, 0, BlockDataLength);
                    // Metadata
                    Array.Copy(data, (BlockDataLength * sectionCount) + (y * NibbleDataLength),
                        chunk.Sections[y].Metadata.Data, 0, NibbleDataLength);
                    // Light
                    Array.Copy(data, ((BlockDataLength + NibbleDataLength) * sectionCount) + (y * NibbleDataLength),
                        chunk.Sections[y].BlockLight.Data, 0, NibbleDataLength);
                    // Sky light
                    if (lightIncluded)
                    {
                        Array.Copy(data, ((BlockDataLength + NibbleDataLength + NibbleDataLength) * sectionCount) + (y * NibbleDataLength),
                            chunk.Sections[y].SkyLight.Data, 0, NibbleDataLength);
                    }
                }
            }
            if (groundUp)
                Array.Copy(data, data.Length - chunk.Biomes.Length, chunk.Biomes, 0, chunk.Biomes.Length);
            client.World.SetChunk(coordinates, chunk);
            //client.OnChunkRecieved(new ChunkRecievedEventArgs(position, new ReadOnlyChunk(chunk)));
        }

        public static void MapChunkBulk(MinecraftClient client, IPacket _packet)
        {
            var packet = (MapChunkBulkPacket)_packet;
            var data = ZlibStream.UncompressBuffer(packet.ChunkData);

            var offset = 0;
            foreach (var metadata in packet.ChunkMetadata)
            {
                var chunkLength = (BlockDataLength + (NibbleDataLength * 2) + (packet.LightIncluded ? NibbleDataLength : 0)) *
                    GetSectionCount(metadata.PrimaryBitMap) +
                        NibbleDataLength * GetSectionCount(metadata.AddBitMap) + (Chunk.Width * Chunk.Depth);

                var chunkData = new byte[chunkLength];
                Array.Copy(data, offset, chunkData, 0, chunkLength);
                AddChunk(client, metadata.ChunkX, metadata.ChunkZ, metadata.PrimaryBitMap, metadata.AddBitMap, packet.LightIncluded, true, chunkData);

                offset += chunkLength;
            }
        }

        public static void BlockChange(MinecraftClient client, IPacket _packet)
        {
          var packet = (BlockChangePacket)_packet;
          var position = new Coordinates3D(packet.X, packet.Y, packet.Z);
          client.World.SetBlockId(position, (short)packet.BlockType);
          client.World.SetMetadata(position, packet.BlockMetadata);
        }

        public static void ChunkData(MinecraftClient client, IPacket _packet)
        {
            var packet = (ChunkDataPacket)_packet;
            if (packet.PrimaryBitMap == 0)
            {
                client.World.RemoveChunk(new Coordinates2D(packet.X, packet.Z));
                return;
            }

            var data = ZlibStream.UncompressBuffer(packet.Data);

            AddChunk(client, packet.X, packet.Z, packet.PrimaryBitMap, packet.AddBitMap, true, packet.GroundUpContinuous, data);
        }

        public static Coordinates2D GetRelativeChunkPosition(Coordinates2D coordinates)
        {
            int regionX = coordinates.X / Region.Width - ((coordinates.X < 0) ? 1 : 0);
            int regionZ = coordinates.Z / Region.Depth - ((coordinates.Z < 0) ? 1 : 0);

            return new Coordinates2D(coordinates.X - regionX * 32, coordinates.Z - regionZ * 32);
        }
    }
}
