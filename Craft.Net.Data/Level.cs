using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Craft.Net.Data.Entities;
using Craft.Net.Data.Generation;
using LibNbt;
using LibNbt.Tags;

namespace Craft.Net.Data
{
    /// <summary>
    /// Represents a Minecraft level
    /// </summary>
    public class Level
    {
        /// <summary>
        /// Craft.Net will use this world generator if it does not recognize the provided one.
        /// </summary>
        public static Type DefaultGenerator = typeof(FlatlandGenerator);

        public World World { get; set; }
        public string Name { get; set; }
        public long Time { get; set; }
        public GameMode GameMode { get; set; }
        public bool MapFeatures { get; set; }
        public IWorldGenerator WorldGenerator { get; set; }
        public long Seed { get; set; }
        public Vector3 SpawnPoint { get; set; }
        public long LastPlayed { get; set; }
        public string LevelDirectory { get; private set; }
        // TODO: Move to weather manager class
        public bool Raining { get; set; }
        public bool Thundering { get; set; }
        public int RainTime { get; set; }
        public int ThunderTime { get; set; }
        
        public Level(string directory)
        {
            LevelDirectory = directory;
            if (!Directory.Exists(LevelDirectory))
                Directory.CreateDirectory(LevelDirectory);
            // Load from level.dat
            if (!File.Exists(Path.Combine(LevelDirectory, "level.dat")))
            {
                WorldGenerator = (IWorldGenerator)Activator.CreateInstance(DefaultGenerator);
                SpawnPoint = WorldGenerator.SpawnPoint;
                World = new World(WorldGenerator, Path.Combine(directory, "region"));
                return;
            }

            NbtFile file = new NbtFile();
            using (var stream = File.Open(Path.Combine(LevelDirectory, "level.dat"), FileMode.Open))
                file.LoadFile(stream, true);
            // TODO: Gracefully handle missing tags
            var data = file.RootTag.Get<NbtCompound>("Data");
            Name = data.Get<NbtString>("LevelName").Value;
            Time = data.Get<NbtLong>("Time").Value;
            GameMode = (GameMode)data.Get<NbtInt>("GameType").Value;
            MapFeatures = data.Get<NbtByte>("MapFeatures").Value == 1;
            Seed = data.Get<NbtLong>("RandomSeed").Value;

            // Find world generator
            string generatorName = data.Get<NbtString>("generatorName").Value;
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t =>
                !t.IsAbstract && !t.IsInterface && typeof(IWorldGenerator).IsAssignableFrom(t)))
            {
                var generator = (IWorldGenerator)Activator.CreateInstance(type);
                if (generator.GeneratorName == generatorName)
                    WorldGenerator = generator;
            }
            if (WorldGenerator == null)
                WorldGenerator = (IWorldGenerator)Activator.CreateInstance(DefaultGenerator);
            WorldGenerator.Seed = Seed;

            int x, y, z;
            x = data.Get<NbtInt>("SpawnX").Value;
            y = data.Get<NbtInt>("SpawnY").Value;
            z = data.Get<NbtInt>("SpawnZ").Value;
            SpawnPoint = new Vector3(x, y, z);

            World = new World(WorldGenerator, Path.Combine(directory, "region"));

            // Move spawn point
            var chunk = World.GetChunk(World.WorldToChunkCoordinates(SpawnPoint));
            var relativeSpawn = World.FindBlockPosition(SpawnPoint);
            SpawnPoint = new Vector3(SpawnPoint.X, chunk.GetHeight((byte)relativeSpawn.X, (byte)relativeSpawn.Z), SpawnPoint.Z);
        }

        public Level(IWorldGenerator worldGenerator, string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            WorldGenerator = worldGenerator;
            SpawnPoint = WorldGenerator.SpawnPoint;
            World = new World(WorldGenerator, Path.Combine(directory, "region"));
        }
    }
}
