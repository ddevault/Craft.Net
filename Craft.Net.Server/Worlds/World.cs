using System;
using Craft.Net.Server.Worlds.Generation;

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
        public Region[] Regions;
        public EntityManager EntityManager;
        public string Name;
        public Vector3 SpawnPoint;
        public GameMode GameMode;
        public Difficulty Difficulty;
        public IWorldGenerator WorldGenerator;

        public World(IWorldGenerator WorldGenerator)
        {
            EntityManager = new EntityManager();
            Name = "world";
            SpawnPoint = Vector3.Zero;
            GameMode = GameMode.Creative;
            Difficulty = Difficulty.Peaceful;
            this.WorldGenerator = WorldGenerator;
        }
    }
}

