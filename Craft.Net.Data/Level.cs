using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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
        public static IWorldGenerator DefaultGenerator = new FlatlandGenerator();
        /// <summary>
        /// The time between automatic saves.
        /// Default value is one minute.
        /// </summary>
        public TimeSpan SaveInterval { get; set; }

        private Timer saveTimer { get; set; }

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
            Name = "world";
            LevelDirectory = directory;
            if (!Directory.Exists(LevelDirectory))
                Directory.CreateDirectory(LevelDirectory);
            // Load from level.dat
            if (!File.Exists(Path.Combine(LevelDirectory, "level.dat")))
            {
                WorldGenerator = DefaultGenerator;
                SpawnPoint = WorldGenerator.SpawnPoint;
                World = new World(WorldGenerator, Path.Combine(directory, "region"));

                SaveInterval = TimeSpan.FromSeconds(5);
                saveTimer = new Timer(Save, null, (int)SaveInterval.TotalMilliseconds, Timeout.Infinite);
                return;
            }

            LoadFromFile(directory);

            // Move spawn point
            var chunk = World.GetChunk(World.WorldToChunkCoordinates(SpawnPoint));
            var relativeSpawn = World.FindBlockPosition(SpawnPoint);
            SpawnPoint = new Vector3(SpawnPoint.X, chunk.GetHeight((byte)relativeSpawn.X, (byte)relativeSpawn.Z), SpawnPoint.Z);

            SaveInterval = TimeSpan.FromSeconds(5);
            saveTimer = new Timer(Save, null, (int)SaveInterval.TotalMilliseconds, Timeout.Infinite);
        }

        public Level(IWorldGenerator worldGenerator, string directory)
        {
            Name = "world";
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            LevelDirectory = directory;
            WorldGenerator = worldGenerator;
            SpawnPoint = WorldGenerator.SpawnPoint;
            World = new World(WorldGenerator, Path.Combine(directory, "region"));
            SaveInterval = TimeSpan.FromSeconds(5);
            saveTimer = new Timer(Save, null, (int)SaveInterval.TotalMilliseconds, Timeout.Infinite);
        }

        private void Save(object discarded)
        {
            Save();
            saveTimer = new Timer(Save, null, (int)SaveInterval.TotalMilliseconds, Timeout.Infinite);
        }

        public void Save()
        {
            NbtFile file = new NbtFile();
            NbtCompound data = new NbtCompound("Data");
            data.Tags.Add(new NbtByte("raining", (byte)(Raining ? 1 : 0)));
            data.Tags.Add(new NbtInt("generatorVersion", 0)); // TODO
            data.Tags.Add(new NbtLong("Time", Time));
            data.Tags.Add(new NbtInt("GameType", (int)GameMode));
            data.Tags.Add(new NbtByte("MapFeatures", (byte)(MapFeatures ? 1 : 0))); // TODO: Move to world generator
            data.Tags.Add(new NbtString("generatorName", WorldGenerator.GeneratorName));
            data.Tags.Add(new NbtByte("initialized", 1));
            data.Tags.Add(new NbtByte("hardcore", 0)); // TODO
            data.Tags.Add(new NbtLong("RandomSeed", Seed));
            data.Tags.Add(new NbtInt("SpawnX", (int)SpawnPoint.X));
            data.Tags.Add(new NbtInt("SpawnY", (int)SpawnPoint.Y));
            data.Tags.Add(new NbtInt("SpawnZ", (int)SpawnPoint.Z));
            data.Tags.Add(new NbtLong("SizeOnDisk", 0));
            data.Tags.Add(new NbtInt("thunderTime", ThunderTime));
            data.Tags.Add(new NbtInt("rainTime", RainTime));
            data.Tags.Add(new NbtInt("version", 19133));
            data.Tags.Add(new NbtByte("thundering", (byte)(Thundering ? 1 : 0)));
            data.Tags.Add(new NbtString("LevelName", Name));
            data.Tags.Add(new NbtLong("LastPlayed", DateTime.UtcNow.Ticks));
            file.RootTag = new NbtCompound();
            file.RootTag.Tags.Add(data);
            using (var stream = File.Open(Path.Combine(LevelDirectory, "level.dat"), FileMode.Create))
                file.SaveFile(stream, true);

            World.Save();
        }

        public static IWorldGenerator GetGenerator(string generatorName)
        {
            IWorldGenerator worldGenerator = null;
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t =>
                !t.IsAbstract && !t.IsInterface && typeof(IWorldGenerator).IsAssignableFrom(t)))
            {
                var generator = (IWorldGenerator)Activator.CreateInstance(type);
                if (generator.GeneratorName == generatorName)
                    worldGenerator = generator;
            }
            if (worldGenerator == null)
                worldGenerator = DefaultGenerator;
            return worldGenerator;
        }

        private void LoadFromFile(string directory)
        {
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
            WorldGenerator = GetGenerator(generatorName);
            WorldGenerator.Seed = Seed;

            int x, y, z;
            x = data.Get<NbtInt>("SpawnX").Value;
            y = data.Get<NbtInt>("SpawnY").Value;
            z = data.Get<NbtInt>("SpawnZ").Value;
            SpawnPoint = new Vector3(x, y, z);

            World = new World(WorldGenerator, Path.Combine(directory, "region"));
        }
    }
}
