using System;
using System.Collections.Generic;
using Craft.Net.Data.Blocks;
using Craft.Net.Data.Generation;
using Craft.Net.Data.Entities;
using System.IO;

namespace Craft.Net.Data
{
    /// <summary>
    /// Represents a horizontally infinite world of blocks with a fixed height of 256 blocks.
    /// </summary>
    public class World
    {
        /// <summary>
        /// The difficulty of this world.
        /// </summary>
        public Difficulty Difficulty { get; set; }
        /// <summary>
        /// The default game mode players use.
        /// </summary>
        public GameMode GameMode { get; set; }
        /// <summary>
        /// The name of this world.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The currently loaded regions for this world.
        /// </summary>
        public Dictionary<Vector3, Region> Regions { get; set; }
        /// <summary>
        /// The entities currently present in this world.
        /// </summary>
        public List<Entity> Entities { get; set; }
        /// <summary>
        /// The seed used to generate this world.
        /// </summary>
        public long Seed { get; set; }
        /// <summary>
        /// The spawn point for new players in this world.
        /// </summary>
        public Vector3 SpawnPoint { get; set; }
        /// <summary>
        /// The world generator used to create this world.
        /// </summary>
        public IWorldGenerator WorldGenerator { get; set; }
        /// <summary>
        /// Gets the directory this world uses to save and load the world.
        /// </summary>
        public string Directory { get; private set; }

        /// <summary>
        /// Creates a new world for client-side use.
        /// </summary>
        public World()
        {
            Name = "world";
            GameMode = GameMode.Creative;
            Difficulty = Difficulty.Peaceful;
            SpawnPoint = Vector3.Zero;
            Seed = DataUtility.Random.Next();
            Entities = new List<Entity>();
            Regions = new Dictionary<Vector3, Region>();
        }

        /// <summary>
        /// Creates a new world for server-side use with the specified world generator.
        /// </summary>
        public World(IWorldGenerator worldGenerator)
        {
            Name = "world";
            GameMode = GameMode.Creative;
            Difficulty = Difficulty.Peaceful;
            WorldGenerator = worldGenerator;
            SpawnPoint = worldGenerator.SpawnPoint;
            Seed = DataUtility.Random.Next();
            Entities = new List<Entity>();
            Regions = new Dictionary<Vector3, Region>();
        }

        /// <summary>
        /// Creates a new world for server-side use with the specified world generator
        /// and the specified working directory.
        /// </summary>
        public World(IWorldGenerator worldGenerator, string directory)
        {
            Name = "world";
            GameMode = GameMode.Creative;
            Difficulty = Difficulty.Peaceful;
            WorldGenerator = worldGenerator;
            SpawnPoint = worldGenerator.SpawnPoint;
            Seed = DataUtility.Random.Next();
            Entities = new List<Entity>();
            Regions = new Dictionary<Vector3, Region>();
            Directory = directory;
        }

        /// <summary>
        /// Creates a new world for server-side use with the specified world generator and seed.
        /// </summary>
        /// <param name="worldGenerator"></param>
        /// <param name="seed"></param>
        public World(IWorldGenerator worldGenerator, long seed) : this(worldGenerator)
        {
            Seed = seed;
        }

        /// <summary>
        /// Gets the level type this world's generator produces.
        /// </summary>
        public string LevelType
        {
            get { return WorldGenerator.LevelType; }
        }

        /// <summary>
        /// Fires when a block in the world is changed.
        /// </summary>
        public event EventHandler<BlockChangedEventArgs> OnBlockChanged;

        /// <summary>
        /// Returns the chunk at the specific position
        /// </summary>
        /// <param name="position">Position in chunk coordinates</param>
        public Chunk GetChunk(Vector3 position)
        {
            //In chunks
            var x = (int)position.X;
            var z = (int)position.Z;

            //In regions
            int regionX = x/Region.Width - ((x < 0) ? 1 : 0);
            int regionZ = z/Region.Depth - ((z < 0) ? 1 : 0);

            var region = CreateOrLoadRegion(new Vector3(regionX, 0, regionZ));
            return region.GetChunk(new Vector3(x - regionX * 32, 0, z - regionZ * 32));
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
            int regionX = x/Region.Width - ((x < 0) ? 1 : 0);
            int regionZ = z/Region.Depth - ((z < 0) ? 1 : 0);

            var region = CreateOrLoadRegion(new Vector3(regionX, 0, regionZ));
            region.SetChunk(new Vector3(x - regionX * 32, 0, z - regionZ * 32), chunk);
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
        /// Sets the block at the specified position.
        /// </summary>
        public void SetBlock(Vector3 position, Block value)
        {
            Chunk chunk;
            Vector3 blockPosition = FindBlockPosition(position, out chunk);

            chunk.SetBlock(blockPosition, value);

            if (OnBlockChanged != null)
                OnBlockChanged(this, new BlockChangedEventArgs(this, position, value));
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
            return new Vector3(x - chunkX * Chunk.Width, y, z - chunkZ * Chunk.Depth);
        }

        private Region CreateOrLoadRegion(Vector3 position)
        {
            if (!Regions.ContainsKey(position))
            {
                // Attempt to load from file
                if (Directory != null && File.Exists(Path.Combine(Directory, Region.GetRegionFileName(position))))
                    Regions.Add(position, new Region(position, WorldGenerator, Path.Combine(Directory, Region.GetRegionFileName(position))));
                else // Generate new region
                    Regions.Add(position, new Region(position, WorldGenerator));
            }
            return Regions[position];
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
    }
}