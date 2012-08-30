using System;
using System.Collections.Generic;
using System.IO;
using Craft.Net.Data.Blocks;
using Craft.Net.Data.Generation;
using Ionic.Zlib;
using LibNbt;

namespace Craft.Net.Data
{
    /// <summary>
    /// Represents a 32x32 area of <see cref="Chunk"/> objects.
    /// Not all of these chunks are represented at any given time, and
    /// will be loaded from disk or generated when the need arises.
    /// </summary>
    public class Region
    {
        // In chunks
        public const int Width = 32, Depth = 32;

        /// <summary>
        /// The currently loaded chunk list.
        /// </summary>
        public Dictionary<Vector3, Chunk> Chunks { get; set; }
        /// <summary>
        /// The location of this region in the overworld.
        /// </summary>
        public Vector3 Position { get; set; }
        /// <summary>
        /// The <see cref="IWorldGenerator"/> used to generate this world.
        /// </summary>
        public IWorldGenerator WorldGenerator { get; set; }

        private Stream regionFile { get; set; }

        /// <summary>
        /// Creates a new Region for server-side use at the given position using
        /// the provided terrain generator.
        /// </summary>
        public Region(Vector3 position, IWorldGenerator worldGenerator)
        {
            Chunks = new Dictionary<Vector3, Chunk>();
            this.Position = position;
            this.WorldGenerator = worldGenerator;
        }

        /// <summary>
        /// Creates a new Region for client-side use at the given position.
        /// </summary>
        public Region(Vector3 position)
        {
            Chunks = new Dictionary<Vector3, Chunk>();
            this.Position = position;
            WorldGenerator = null;
        }

        /// <summary>
        /// Creates a region from the given region file.
        /// </summary>
        public Region(Vector3 position, IWorldGenerator worldGenerator, string file) : this(position, worldGenerator)
        {
            if (File.Exists(file))
                regionFile = File.Open(file, FileMode.Open);
            else
            {
                // TODO: Create region file
            }
        }

        /// <summary>
        /// Retrieves the requested chunk from the region, or
        /// generates it if a world generator is provided.
        /// </summary>
        /// <param name="position">The position of the requested local chunk coordinates.</param>
        public Chunk GetChunk(Vector3 position)
        {
            // TODO: This could use some refactoring
            lock (Chunks)
            {
                if (!Chunks.ContainsKey(position))
                {
                    if (regionFile != null)
                    {
                        // Search the stream for that region
                        lock (regionFile)
                        {
                            var chunkData = GetChunkFromTable(position);
                            if (chunkData == null)
                            {
                                if (WorldGenerator == null)
                                    throw new ArgumentException("The requested chunk is not loaded.", "position");
                                Chunks.Add(position, WorldGenerator.GenerateChunk(position, this));
                                return Chunks[position];
                            }
                            regionFile.Seek(chunkData.Item1, SeekOrigin.Begin);
                            int length = DataUtility.ReadInt32(regionFile);
                            int compressionMode = regionFile.ReadByte();
                            switch (compressionMode)
                            {
                                case 1: // gzip
                                    break;
                                case 2: // zlib
                                    byte[] compressed = new byte[length];
                                    regionFile.Read(compressed, 0, compressed.Length);
                                    byte[] uncompressed = ZlibStream.UncompressBuffer(compressed);
                                    MemoryStream memoryStream = new MemoryStream(uncompressed);
                                    NbtFile nbt = new NbtFile();
                                    nbt.LoadFile(memoryStream, false);
                                    var chunk = Chunk.FromNbt(position, nbt);
                                    chunk.ParentRegion = this;
                                    Chunks.Add(position, chunk);
                                    break;
                                default:
                                    throw new InvalidDataException("Invalid compression scheme provided by region file.");
                            }
                        }
                    }
                    else if (WorldGenerator == null)
                        throw new ArgumentException("The requested chunk is not loaded.", "position");
                    else
                        Chunks.Add(position, WorldGenerator.GenerateChunk(position, this));
                }
                return Chunks[position];
            }
        }

        /// <summary>
        /// Sets the chunk at the specified local position to the given value.
        /// </summary>
        public void SetChunk(Vector3 position, Chunk chunk)
        {
            if (!Chunks.ContainsKey(position))
                Chunks.Add(position, chunk);
            Chunks[position] = chunk;
        }

        /// <summary>
        /// Gets the block at the given local position.
        /// </summary>
        public Block GetBlock(Vector3 position)
        {
            position = position.Floor();
            Vector3 relativePosition = position;
            position.X = (int)(position.X) / Chunk.Width;
            position.Y = 0;
            position.Z = (int)(position.Z) / Chunk.Depth;

            relativePosition.X = (int)(relativePosition.X) % Chunk.Width;
            relativePosition.Y = 0;
            relativePosition.Z = (int)(relativePosition.Z) % Chunk.Depth;

            if (!Chunks.ContainsKey(position))
                Chunks.Add(position, WorldGenerator.GenerateChunk(position, this));

            return Chunks[position].GetBlock(relativePosition);
        }

        /// <summary>
        /// Sets the block at the given local position.
        /// </summary>
        public void SetBlock(Vector3 position, Block value)
        {
            position = position.Floor();
            Vector3 relativePosition = position;
            position.X = (int)(position.X)/Chunk.Width;
            position.Y = 0;
            position.Z = (int)(position.Z)/Chunk.Depth;

            relativePosition.X = (int)(relativePosition.X) % Chunk.Width;
            relativePosition.Z = (int)(relativePosition.Z) % Chunk.Depth;

            if (!Chunks.ContainsKey(position))
                Chunks.Add(position, WorldGenerator.GenerateChunk(position, this));

            Chunks[position].SetBlock(relativePosition, value);
        }

        #region Stream Helpers

        private const int ChunkSizeMultiplier = 4096;
        private Tuple<int, int> GetChunkFromTable(Vector3 position) // <offset, length>
        {
            int tableOffset = (((int)(position.X) % Width) +
                               ((int)(position.Z) % Depth) * Width) * 4;
            regionFile.Seek(tableOffset, SeekOrigin.Begin);
            byte[] offsetBuffer = new byte[4];
            regionFile.Read(offsetBuffer, 0, 3);
            Array.Reverse(offsetBuffer);
            int length = regionFile.ReadByte();
            int offset = BitConverter.ToInt32(offsetBuffer, 0) << 4;
            if (offset == 0 || length == 0)
                return null;
            return new Tuple<int, int>(offset,
                length * ChunkSizeMultiplier);
        }

        #endregion

        public static string GetRegionFileName(Vector3 position)
        {
            int x = (int)(position.X / 32);
            int z = (int)(position.Z / 32);
            return "r." + x + "." + z + ".mcr";
        }
    }
}