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
            Regions = new Dictionary<Vector3, Region>();
        }

        public World(IWorldGenerator WorldGenerator, long Seed) : this(WorldGenerator)
        {
            this.Seed = Seed;
        }

        public Block GetBlock(Vector3 position)
        {
            position = position.Floor();
            // TODO: Is there a better way of doing this?
            bool xNegative = position.X < 0;
            bool zNegative = position.Z < 0;
            if (xNegative)
                position.X = -position.X;
            if (zNegative)
                position.Z = -position.Z;

            Vector3 relativePosition = position;

            position.X = (int)(position.X) % (Region.Width * Chunk.Width);
            position.Y = 0;
            position.Z = (int)(position.Z) % (Region.Depth * Chunk.Depth);

            if (xNegative)
                position.X = -position.X;
            if (zNegative)
                position.Z = -position.Z;

            if (!Regions.ContainsKey(position))
                Regions.Add(position, new Region(position, WorldGenerator));

            if (xNegative)
                relativePosition.X = (Region.Width * Chunk.Width) - relativePosition.X;
            if (zNegative)
                relativePosition.Z = (Region.Depth * Chunk.Depth) - relativePosition.Z;

            return Regions[position].GetBlock(relativePosition);
        }

        public void SetBlock(Vector3 position, Block value)
        {
            position = position.Floor();
            // TODO: Is there a better way of doing this?
            bool xNegative = position.X < 0;
            bool zNegative = position.Z < 0;
            if (xNegative)
                position.X = -position.X;
            if (zNegative)
                position.Z = -position.Z;
            
            Vector3 relativePosition = position;
            
            position.X = (int)(position.X) % (Region.Width * Chunk.Width);
            position.Y = 0;
            position.Z = (int)(position.Z) % (Region.Depth * Chunk.Depth);
            
            if (xNegative)
                position.X = -position.X;
            if (zNegative)
                position.Z = -position.Z;
            
            if (!Regions.ContainsKey(position))
                Regions.Add(position, new Region(position, WorldGenerator));
            
            if (xNegative)
                relativePosition.X = (Region.Width * Chunk.Width) - relativePosition.X;
            if (zNegative)
                relativePosition.Z = (Region.Depth * Chunk.Depth) - relativePosition.Z;
            
            Regions[position].SetBlock(relativePosition, value);
        }
    }
}

