using System;

namespace Craft.Net.Server.Worlds
{
    public class World
    {
        public string LevelType;
        public Region[] Regions;
        public EntityManager EntityManager;
        public string Name;
        public Vector3 SpawnPoint;
        public GameMode GameMode;
        public Difficulty Difficulty;

        public World()
        {
            LevelType = "DEFAULT";
            EntityManager = new EntityManager();
            Name = "world";
            SpawnPoint = Vector3.Zero;
            GameMode = GameMode.Creative;
            Difficulty = Difficulty.Peaceful;
        }
    }
}

