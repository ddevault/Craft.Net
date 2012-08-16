using System;
using System.Collections.Generic;
using Craft.Net.Data.Blocks;
using Craft.Net.Data.Generation;

namespace Craft.Net.Data
{
    public class World
    {
        public Difficulty Difficulty;
        public GameMode GameMode;
        public string Name;
        public Dictionary<Vector3, Region> Regions;
        public long Seed;
        public Vector3 SpawnPoint;
        public IWorldGenerator WorldGenerator;

        public World(IWorldGenerator worldGenerator)
        {
            Name = "world";
            GameMode = GameMode.Creative;
            Difficulty = Difficulty.Peaceful;
            this.WorldGenerator = worldGenerator;
            SpawnPoint = worldGenerator.SpawnPoint;
            Seed = DataUtility.Random.Next();
            Regions = new Dictionary<Vector3, Region>();
        }

        public World(IWorldGenerator worldGenerator, long seed) : this(worldGenerator)
        {
            this.Seed = seed;
        }

        public string LevelType
        {
            get { return WorldGenerator.LevelType; }
        }

        public event EventHandler<BlockChangedEventArgs> OnBlockChanged;

        /// <summary>
        /// Returns the chunk at the specific position
        /// </summary>
        /// <param name="position">Position in chunk coordinates</param>
        /// <returns></returns>
        public Chunk GetChunk(Vector3 position)
        {
            //In chunks
            var x = (int)position.X;
            var z = (int)position.Z;

            //In regions
            int regionX = x/Region.Width - ((x < 0) ? 1 : 0);
            int regionZ = z/Region.Depth - ((z < 0) ? 1 : 0);

            var region = new Vector3(regionX, 0, regionZ);
            if (!Regions.ContainsKey(region))
                Regions.Add(region, new Region(region, WorldGenerator));

            return Regions[region].GetChunk(new Vector3(x - regionX*32, 0, z - regionZ*32));
        }

        /// <summary>
        /// Sets the chunk at the given position to the chunk provided.
        /// </summary>
        /// <param name="position">Position in chunk coordinates</param>
        public void SetChunk(Vector3 position, Chunk chunk)
        {
            //In chunks
            var x = (int)position.X;
            var z = (int)position.Z;

            //In regions
            int regionX = x/Region.Width - ((x < 0) ? 1 : 0);
            int regionZ = z/Region.Depth - ((z < 0) ? 1 : 0);

            var region = new Vector3(regionX, 0, regionZ);
            if (!Regions.ContainsKey(region))
                Regions.Add(region, new Region(region, WorldGenerator));

            Regions[region].SetChunk(new Vector3(x - regionX*32, 0, z - regionZ*32), chunk);
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
            var x = (int)position.X;
            var y = (int)position.Y;
            var z = (int)position.Z;

            if (y < 0 || y >= Chunk.Height) throw new ArgumentOutOfRangeException("Block is out of range");

            int chunkX = x/(Chunk.Width) - ((x < 0) ? 1 : 0);
            int chunkZ = z/(Chunk.Depth) - ((z < 0) ? 1 : 0);

            chunk = GetChunk(new Vector3(chunkX, 0, chunkZ));
            return new Vector3(x - chunkX*Chunk.Width, y, z - chunkZ*Chunk.Depth);
            ;
        }
    }
}