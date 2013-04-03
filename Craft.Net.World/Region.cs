using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Craft.Net.Utilities;
using fNbt;
using Ionic.Zlib;

namespace Craft.Net.World
{
    /// <summary>
    /// Represents a 32x32 area of <see cref="Chunk"/> objects.
    /// Not all of these chunks are represented at any given time, and
    /// will be loaded from disk or generated when the need arises.
    /// </summary>
    public class Region : IDisposable
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

        public World World { get; set; }

        private Stream regionFile { get; set; }

        /// <summary>
        /// Creates a new Region for server-side use at the given position using
        /// the provided terrain generator.
        /// </summary>
        public Region(Vector3 position, World world)
        {
            Chunks = new Dictionary<Vector3, Chunk>();
            Position = position;
            World = world;
            WorldGenerator = world.WorldGenerator;
        }

        /// <summary>
        /// Creates a region from the given region file.
        /// </summary>
        public Region(Vector3 position, World world, string file) : this(position, world)
        {
            if (File.Exists(file))
                regionFile = File.Open(file, FileMode.OpenOrCreate);
            else
            {
                regionFile = File.Open(file, FileMode.OpenOrCreate);
                CreateRegionHeader();
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
                                GenerateChunk(position);
                                return Chunks[position];
                            }
                            regionFile.Seek(chunkData.Item1, SeekOrigin.Begin);
                            int length = new MinecraftStream(regionFile).ReadInt32(); // TODO: Avoid making new objects here, and in the WriteInt32
                            int compressionMode = regionFile.ReadByte();
                            switch (compressionMode)
                            {
                                case 1: // gzip
                                    break;
                                case 2: // zlib
                                    var nbt = new NbtFile();
                                    nbt.LoadFromStream(regionFile, NbtCompression.ZLib, null);
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
        /// Retrieves the requested chunk from the region, without using the
        /// world generator if it does not exist.
        /// </summary>
        /// <param name="position">The position of the requested local chunk coordinates.</param>
        public Chunk GetChunkWithoutGeneration(Vector3 position)
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
                                return null;
                            regionFile.Seek(chunkData.Item1, SeekOrigin.Begin);
                            int length = new MinecraftStream(regionFile).ReadInt32(); // TODO: Avoid making new objects here, and in the WriteInt32
                            int compressionMode = regionFile.ReadByte();
                            switch (compressionMode)
                            {
                                case 1: // gzip
                                    break;
                                case 2: // zlib
                                    var nbt = new NbtFile();
                                    nbt.LoadFromStream(regionFile, NbtCompression.ZLib, null);
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

        private void GenerateChunk(Vector3 position)
        {
            var chunk = WorldGenerator.GenerateChunk(position, this);
            Chunks.Add(position, chunk);
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

        ///// <summary>
        ///// Gets the block at the given local position.
        ///// </summary>
        //public Block GetBlock(Vector3 position)
        //{
        //    position = position.Floor();
        //    Vector3 relativePosition = position;
        //    position.X = (int)(position.X) / Chunk.Width;
        //    position.Y = 0;
        //    position.Z = (int)(position.Z) / Chunk.Depth;

        //    relativePosition.X = (int)(relativePosition.X) % Chunk.Width;
        //    relativePosition.Y = 0;
        //    relativePosition.Z = (int)(relativePosition.Z) % Chunk.Depth;

        //    if (!Chunks.ContainsKey(position))
        //        Chunks.Add(position, WorldGenerator.GenerateChunk(position, this));

        //    return Chunks[position].GetBlock(relativePosition);
        //}

        ///// <summary>
        ///// Sets the block at the given local position.
        ///// </summary>
        //public void SetBlock(Vector3 position, Block value)
        //{
        //    position = position.Floor();
        //    Vector3 relativePosition = position;
        //    position.X = (int)(position.X) / Chunk.Width;
        //    position.Y = 0;
        //    position.Z = (int)(position.Z) / Chunk.Depth;

        //    relativePosition.X = (int)(relativePosition.X) % Chunk.Width;
        //    relativePosition.Z = (int)(relativePosition.Z) % Chunk.Depth;

        //    if (!Chunks.ContainsKey(position))
        //        Chunks.Add(position, WorldGenerator.GenerateChunk(position, this));

        //    Chunks[position].SetBlock(relativePosition, value);
        //}

        /// <summary>
        /// Saves this region to the specified file.
        /// </summary>
        public void Save(string file)
        {
            if (regionFile != null)
                throw new InvalidOperationException("This object is already associated with a region file, use Save()");
            if (File.Exists(file))
                regionFile = File.Open(file, FileMode.OpenOrCreate);
            else
            {
                regionFile = File.Open(file, FileMode.OpenOrCreate);
                CreateRegionHeader();
            }
            Save();
        }

        /// <summary>
        /// Saves this region to the open region file.
        /// </summary>
        public void Save()
        {
            lock (Chunks)
            {
                lock (regionFile)
                {
                    foreach (var kvp in  Chunks)
                    {
                        var chunk = kvp.Value;
                        if (chunk.IsModified)
                        {
                            var data = chunk.ToNbt();
                            byte[] raw = data.SaveToBuffer(NbtCompression.ZLib);

                            var header = GetChunkFromTable(kvp.Key);
                            if (header == null || header.Item2 > raw.Length)
                                header = AllocateNewChunks(kvp.Key, raw.Length);

                            regionFile.Seek(header.Item1, SeekOrigin.Begin);
                            new MinecraftStream(regionFile).WriteInt32(raw.Length);
                            regionFile.WriteByte(2); // Compressed with zlib
                            regionFile.Write(raw, 0, raw.Length);
                        }
                    }
                    regionFile.Flush();
                }
            }
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

        private void CreateRegionHeader()
        {
            regionFile.Write(new byte[8192], 0, 8192);
            regionFile.Flush();
        }

        private Tuple<int, int> AllocateNewChunks(Vector3 position, int length)
        {
            // Expand region file
            regionFile.Seek(0, SeekOrigin.End);
            int dataOffset = (int)regionFile.Position;

            length /= ChunkSizeMultiplier;
            length++;
            regionFile.Write(new byte[length * ChunkSizeMultiplier], 0, length * ChunkSizeMultiplier);

            // Write table entry
            int tableOffset = (((int)(position.X) % Width) +
                               ((int)(position.Z) % Depth) * Width) * 4;
            regionFile.Seek(tableOffset, SeekOrigin.Begin);

            byte[] entry = BitConverter.GetBytes(dataOffset >> 4);
            entry[0] = (byte)length;
            Array.Reverse(entry);
            regionFile.Write(entry, 0, entry.Length);

            return new Tuple<int, int>(dataOffset, length * ChunkSizeMultiplier);
        }

        #endregion

        public static string GetRegionFileName(Vector3 position)
        {
            var x = (int)position.X;
            var z = (int)position.Z;
            return "r." + x + "." + z + ".mca";
        }

        public void UnloadChunk(Vector3 position)
        {
            Chunks.Remove(position);
        }

        public void Dispose()
        {
            lock (regionFile)
            {
                regionFile.Flush();
                regionFile.Close();
            }
        }
    }
}