using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using Craft.Net.Utilities;

namespace Craft.Net.World
{
    /// <summary>
    /// Represents a horizontally infinite world of blocks with a fixed height of 256 blocks.
    /// </summary>
    public class World
    {
        public const int Height = 256;

        public string Name { get; set; }
        public Dictionary<Vector3, Region> Regions { get; set; }
        public IWorldGenerator Generator { get; set; }
        public string Directory { get; private set; }
        public Level Level { get; set; }

        public World()
        {
            
        }

        public Chunk GetChunk(int x, int z)
        {
            // Region coordinates
            int regionX = x / Region.Width - ((x < 0) ? 1 : 0);
            int regionZ = z / Region.Depth - ((z < 0) ? 1 : 0);

            var region = CreateOrLoadRegion(new Vector3(regionX, 0, regionZ));
            return region.GetChunk(new Vector3(x - regionX * 32, 0, z - regionZ * 32));
        }

        public Chunk GetChunkWithoutGeneration(int x, int z)
        {
            // Region coordinates
            int regionX = x / Region.Width - ((x < 0) ? 1 : 0);
            int regionZ = z / Region.Depth - ((z < 0) ? 1 : 0);

            var regionPosition = new Vector3(regionX, 0, regionZ);

            if (!Regions.ContainsKey(regionPosition))
                throw new KeyNotFoundException("No region is loaded at that position.");

            return Regions[regionPosition].GetChunkWithoutGeneration(new Vector3(x - regionX * 32, 0, z - regionZ * 32));
        }

        /// <summary>
        /// Sets the chunk at the given position to the chunk provided.
        /// </summary>
        public void SetChunk(Vector3 position, Chunk chunk)
        {
            //In chunks
            var x = (int)position.X;
            var z = (int)position.Z;

            //In regions
            int regionX = x / Region.Width - ((x < 0) ? 1 : 0);
            int regionZ = z / Region.Depth - ((z < 0) ? 1 : 0);

            var region = CreateOrLoadRegion(new Vector3(regionX, 0, regionZ));
            chunk.IsModified = true;
            chunk.ParentRegion = region;
            region.SetChunk(new Vector3(x - regionX * 32, 0, z - regionZ * 32), chunk);
        }

        public void UnloadChunk(int x, int z, bool save)
        {
            //In regions
            int regionX = x / Region.Width - ((x < 0) ? 1 : 0);
            int regionZ = z / Region.Depth - ((z < 0) ? 1 : 0);

            var position = new Vector3(regionX, 0, regionZ);
            if (!Regions.ContainsKey(position))
                return;
            var region = Regions[position];
            if (save)
                region.Save();
            region.UnloadChunk(new Vector3(x - regionX * 32, 0, z - regionZ * 32));
        }

        public void UnloadChunk(int x, int z)
        {
            UnloadChunk(x, z, false);
        }

        /// <summary>
        /// Gets the block at the specified position.
        /// </summary>
        public Block GetBlock(Vector3 position)
        {
            Chunk chunk;
            Vector3 blockPosition = FindBlockPosition(position, out chunk);

            return chunk.GetBlock(blockPosition);
        }

        /// <summary>
        /// Returns air for blocks outside the world without throwing an exception.
        /// </summary>
        public Block SafeGetBlock(Vector3 position)
        {
            if (position.Y < 0 || position.Y >= World.Height)
                return new AirBlock();

            Chunk chunk;
            Vector3 blockPosition = FindBlockPosition(position, out chunk);

            return chunk.GetBlock(blockPosition);
        }

        /// <summary>
        /// Sets the block at the specified position.
        /// </summary>
        public void SetBlock(Vector3 position, Block value)
        {
            Chunk chunk;
            Vector3 blockPosition = FindBlockPosition(position, out chunk);

            chunk.SetBlock(blockPosition, value);
        }

        public void Save()
        {
            lock (Regions)
            {
                foreach (var region in Regions)
                    region.Value.Save();
            }
        }

        public void Save(string path)
        {
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
            Directory = path;
            lock (Regions)
            {
                foreach (var region in Regions)
                    region.Value.Save(Path.Combine(Directory, Region.GetRegionFileName(region.Key)));
            }
        }

        private Vector3 FindBlockPosition(Vector3 position, out Chunk chunk)
        {
            var x = (int)position.X;
            var y = (int)position.Y;
            var z = (int)position.Z;

            if (y < 0 || y >= Chunk.Height)
                throw new ArgumentOutOfRangeException("position", "Position is out of range");

            int chunkX = x / (Chunk.Width) - ((x < 0) ? 1 : 0);
            int chunkZ = z / (Chunk.Depth) - ((z < 0) ? 1 : 0);

            chunk = GetChunk(new Vector3(chunkX, 0, chunkZ));
            return new Vector3((x - chunkX * Chunk.Width) % Chunk.Width, y, (z - chunkZ * Chunk.Depth) % Chunk.Depth);
        }

        private Region CreateOrLoadRegion(Vector3 position)
        {
            lock (Regions)
            {
                if (!Regions.ContainsKey(position))
                {
                    if (Directory == null)
                        Regions.Add(position, new Region(position, this));
                    else
                        Regions.Add(position,
                                new Region(position, this,
                                           Path.Combine(Directory, Region.GetRegionFileName(position))));
                }
                return Regions[position];
            }
        }

        /// <summary>
        /// Gets the coordinates of the chunk that contains the given world coordinates.
        /// </summary>
        public static Vector3 WorldToChunkCoordinates(Vector3 position)
        {
            var x = (int)position.X;
            var y = (int)position.Y;
            var z = (int)position.Z;

            if (y < 0 || y >= Chunk.Height)
                throw new ArgumentOutOfRangeException("position", "Position is out of range");

            int chunkX = x / (Chunk.Width) - ((x < 0) ? 1 : 0);
            int chunkZ = z / (Chunk.Depth) - ((z < 0) ? 1 : 0);

            return new Vector3(chunkX, 0, chunkZ);
        }

        /// <summary>
        /// Gets the position of the given block in world coordinates relative to
        /// its parent chunk.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vector3 FindBlockPosition(Vector3 position)
        {
            var x = (int)position.X;
            var y = (int)position.Y;
            var z = (int)position.Z;

            if (y < 0 || y >= Chunk.Height)
                throw new ArgumentOutOfRangeException("position", "Position is out of range");

            int chunkX = x / (Chunk.Width) - ((x < 0) ? 1 : 0);
            int chunkZ = z / (Chunk.Depth) - ((z < 0) ? 1 : 0);

            return new Vector3(x - chunkX * Chunk.Width, y, z - chunkZ * Chunk.Depth);
        }

        /// <summary>
        /// Returns the position of the specified chunk relative to its parent region.
        /// </summary>
        public static Vector3 GetRelativeChunkPosition(Vector3 position)
        {
            //In chunks
            var x = (int)position.X;
            var z = (int)position.Z;

            //In regions
            int regionX = x / Region.Width - ((x < 0) ? 1 : 0);
            int regionZ = z / Region.Depth - ((z < 0) ? 1 : 0);

            return new Vector3(x - regionX * 32, 0, z - regionZ * 32);
        }

        public static bool IsValidPosition(Vector3 position)
        {
            return position.Y >= 0 && position.Y <= 255;
        }
    }
}