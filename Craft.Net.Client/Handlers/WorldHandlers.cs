using System;
using Craft.Net.Client.Events;
using Ionic.Zlib;
using Craft.Net.Data;
using Craft.Net.Data.Blocks;

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
            var position = new Vector3(x, 0, z);
            var relativePosition = World.GetRelativeChunkPosition(position);
            var chunk = new Chunk(relativePosition);
            var sectionCount = GetSectionCount(primaryBitMap);

            // Run through the sections
            // TODO: Support block IDs >255
            for (int y = 0; y < 16; y++)
            {
                if ((primaryBitMap & (1 << y)) > 0)
                {
                    // Add this section

                    // Blocks
                    Array.Copy(data, y * BlockDataLength, chunk.Sections[y].Blocks, 0, BlockDataLength);

                    // Metadata
                    Array.Copy(data, (BlockDataLength * sectionCount) + (y * NibbleDataLength),
                        chunk.Sections[y].Metadata.Data, 0, NibbleDataLength);

                    // Light
                    Array.Copy(data, ((BlockDataLength + NibbleDataLength) * sectionCount) + (y * NibbleDataLength),
                        chunk.Sections[y].BlockLight.Data, 0, NibbleDataLength);

                    // Sky light
                    // We don't rely on lightIncluded here because it's inconsistent between dimensions
                    if (((BlockDataLength + NibbleDataLength + NibbleDataLength) * sectionCount) + (y * NibbleDataLength) + NibbleDataLength <= data.Length)
                        Array.Copy(data, ((BlockDataLength + NibbleDataLength + NibbleDataLength) * sectionCount) + (y * NibbleDataLength),
                            chunk.Sections[y].SkyLight.Data, 0, NibbleDataLength);
                }
            }

            // biomes
            if (groundUp)
                Array.Copy(data, data.Length - chunk.Biomes.Length, chunk.Biomes, 0, chunk.Biomes.Length);

            client.World.SetChunk(position, chunk);
            client.OnChunkRecieved(new ChunkRecievedEventArgs(position, new ReadOnlyChunk(chunk)));
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

        public static void ChunkData(MinecraftClient client, IPacket _packet)
        {
            var packet = (ChunkDataPacket)_packet;
            if (packet.PrimaryBitMap == 0)
            {
                client.World.RemoveChunk(packet.X, packet.Z);
                return;
            }

            var data = ZlibStream.UncompressBuffer(packet.Data);

            AddChunk(client, packet.X, packet.Z, packet.PrimaryBitMap, packet.AddBitMap, true, packet.GroundUpContinuous, data);
        }

        public static void UpdateSign(MinecraftClient client, IPacket _packet)
        {
            var packet = (UpdateSignPacket)_packet;
            var position = new Vector3(packet.X, packet.Y, packet.Z);
            
            var signTileEntity = new SignTileEntity
            {
                Text1 = packet.Text1,
                Text2 = packet.Text2,
                Text3 = packet.Text3,
                Text4 = packet.Text4
            };

            client.OnSignUpdateReceived(new SignUpdateReceivedEventArgs(position, signTileEntity));

            var chunk = client.World.GetChunk(position);
            if (chunk == null || chunk.Chunk == null) return;

            chunk.Chunk.TileEntities[position] = signTileEntity;
        }
    }
}
