using System;
using Craft.Net.Server.Worlds.Generation;
using Craft.Net.Server.Blocks;
using System.Collections.Generic;
using Craft.Net.Server.Events;

namespace Craft.Net.Server.Worlds
{
    public class World
    {
        public string LevelType
        {
            get
            {
                return WorldGenerator.LevelType;
            }
        }

        public Dictionary<Vector3, Region> Regions;
        public EntityManager EntityManager;
        public string Name;
        public Vector3 SpawnPoint;
        public GameMode GameMode;
        public Difficulty Difficulty;
        public IWorldGenerator WorldGenerator;
        public long Seed;

        public event EventHandler<BlockChangedEventArgs> OnBlockChanged;

        public World(IWorldGenerator WorldGenerator)
        {
            EntityManager = new EntityManager(this);
            Name = "world";
            GameMode = GameMode.Creative;
            Difficulty = Difficulty.Peaceful;
            this.WorldGenerator = WorldGenerator;
            SpawnPoint = WorldGenerator.SpawnPoint;
            Seed = MinecraftServer.Random.Next();
            Regions = new Dictionary<Vector3, Region>();
        }

        public World(IWorldGenerator WorldGenerator, long Seed) : this(WorldGenerator)
        {
            this.Seed = Seed;
        }

        /// <summary>
        /// Returns the chunk at the specific position
        /// </summary>
        /// <param name="position">Position in chunk coordinates</param>
        /// <returns></returns>
        public Chunk GetChunk(Vector3 position)
        {
            //In chunks
            int x = (int)position.X;
            int z = (int)position.Z;

            //In regions
            int regionX = x / Region.Width - ((x < 0) ? 1 : 0);
            int regionZ = z / Region.Depth - ((z < 0) ? 1 : 0);

            Vector3 region = new Vector3(regionX, 0, regionZ);
            if (!Regions.ContainsKey(region))
                Regions.Add(region, new Region(region, WorldGenerator));

            return Regions[region].GetChunk(new Vector3(x - regionX * 32, 0, z - regionZ * 32));
        }

        /// <summary>
        /// Sets the chunk at the given position to the chunk provided.
        /// </summary>
        /// <param name="position">Position in chunk coordinates</param>
        public void SetChunk(Vector3 position, Chunk chunk)
        {
            //In chunks
            int x = (int)position.X;
            int z = (int)position.Z;

            //In regions
            int regionX = x / Region.Width - ((x < 0) ? 1 : 0);
            int regionZ = z / Region.Depth - ((z < 0) ? 1 : 0);

            Vector3 region = new Vector3(regionX, 0, regionZ);
            if (!Regions.ContainsKey(region))
                Regions.Add(region, new Region(region, WorldGenerator));

            Regions[region].SetChunk(new Vector3(x - regionX * 32, 0, z - regionZ * 32), chunk);
        }

        public Block GetBlock(Vector3 position)
        {
            Chunk chunk;
            Vector3 blockPosition = FindBlockPosition(position, out chunk);

            return chunk.GetBlock(blockPosition);
        }

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
            int x = (int)position.X;
            int y = (int)position.Y;
            int z = (int)position.Z;

            if (y < 0 || y >= Chunk.Height) throw new ArgumentOutOfRangeException("Block is out of range");

            int chunkX = x / (Chunk.Width) - ((x < 0) ? 1 : 0);
            int chunkZ = z / (Chunk.Depth) - ((z < 0) ? 1 : 0);

            chunk = GetChunk(new Vector3(chunkX, 0, chunkZ));
            return new Vector3(x - chunkX * Chunk.Width, y, z - chunkZ * Chunk.Depth);;
        }
    }
}

