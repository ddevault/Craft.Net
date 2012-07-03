using System;
using Craft.Net.Server.Worlds.Generation;
using Craft.Net.Server.Blocks;
using System.Collections.Generic;

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

        public World(IWorldGenerator WorldGenerator)
        {
            EntityManager = new EntityManager();
            Name = "world";
            GameMode = GameMode.Creative;
            Difficulty = Difficulty.Peaceful;
            this.WorldGenerator = WorldGenerator;
            SpawnPoint = WorldGenerator.SpawnPoint;
            Seed = MinecraftServer.Random.Next();
        }

        public World(IWorldGenerator WorldGenerator, long Seed) : this(WorldGenerator)
        {
            this.Seed = Seed;
        }

        public Block GetBlock(Vector3 position)
        {
            position = position.Floor();
            return null;
        }
    }
}

