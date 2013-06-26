using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Craft.Net.Data.Entities;
using Craft.Net.Data.Generation;
using Craft.Net.Data.NbtSerialization;
using fNbt;

namespace Craft.Net.Data
{
    /// <summary>
    /// Represents a Minecraft level
    /// </summary>
    public class Level // : INotifyPropertyChanged // TODO
    {
        /// <summary>
        /// Craft.Net will use this world generator if it does not recognize the provided one.
        /// </summary>
        public static IWorldGenerator DefaultGenerator = new FlatlandGenerator();
        public const int TickLength = 1000 / 20;
        /// <summary>
        /// The time between automatic saves.
        /// Default value is one minute.
        /// </summary>
        public static TimeSpan SaveInterval { get; set; }

        private Timer saveTimer { get; set; }
        private Timer tickTimer { get; set; }

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
        public string GeneratorOptions { get; set; }
        // TODO: Move to weather manager class
        public bool Raining { get; set; }
        public bool Thundering { get; set; }
        public int RainTime { get; set; }
        public int ThunderTime { get; set; }
        public Difficulty Difficulty { get; set; }
        /// <summary>
        /// If set, PlayerName's player.dat file will be copied into level.dat
        /// under the Player compound, which allows for use in singleplayer.
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// Default constructor for a level that only exists in memory.
        /// </summary>
        public Level()
        {
            World = new World(this);
            Name = "world";
            GameMode = GameMode.Survival;
            MapFeatures = false;
            WorldGenerator = DefaultGenerator;
            Seed = GenerateSeed();
            WorldGenerator.Seed = Seed;
            WorldGenerator.Initialize(this);
            SpawnPoint = WorldGenerator.SpawnPoint;
            tickTimer = new Timer(Tick, null, TickLength, TickLength);
            Difficulty = Difficulty.Normal;
        }

        public Level(World world)
        {
            World = world;
            Name = "world";
            GameMode = GameMode.Survival;
            MapFeatures = false;
            Difficulty = Difficulty.Normal;
        }

        public Level(IWorldGenerator generator) : this()
        {
            WorldGenerator = generator;
            generator.Initialize(this);
            World.WorldGenerator = WorldGenerator;
        }

        // TODO: Refactor constructors
        public Level(string directory)
        {
            Difficulty = Difficulty.Normal;
            Name = "world";
            LevelDirectory = directory;
            if (!Directory.Exists(LevelDirectory))
                Directory.CreateDirectory(LevelDirectory);
            // Load from level.dat
            if (!File.Exists(Path.Combine(LevelDirectory, "level.dat")))
            {
                WorldGenerator = DefaultGenerator;
                WorldGenerator.Initialize(this);
                SpawnPoint = WorldGenerator.SpawnPoint;
                World = new World(this, WorldGenerator, Path.Combine(directory, "region"));

                SaveInterval = TimeSpan.FromSeconds(5);
                saveTimer = new Timer(Save, null, (int)SaveInterval.TotalMilliseconds, Timeout.Infinite);
                tickTimer = new Timer(Tick, null, TickLength, TickLength);
                return;
            }

            LoadFromFile(directory);

            // Move spawn point
            var chunk = World.GetChunk(World.WorldToChunkCoordinates(SpawnPoint));
            var relativeSpawn = World.FindBlockPosition(SpawnPoint);
            SpawnPoint = new Vector3(SpawnPoint.X, chunk.GetHeight((byte)relativeSpawn.X, (byte)relativeSpawn.Z), SpawnPoint.Z);

            SaveInterval = TimeSpan.FromSeconds(5);
            saveTimer = new Timer(Save, null, (int)SaveInterval.TotalMilliseconds, Timeout.Infinite);
            tickTimer = new Timer(Tick, null, TickLength, TickLength);
        }

        public Level(IWorldGenerator worldGenerator, string directory)
        {
            Difficulty = Difficulty.Normal;
            Name = "world";
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            LevelDirectory = directory;
            WorldGenerator = worldGenerator;
            WorldGenerator.Initialize(this);
            SpawnPoint = WorldGenerator.SpawnPoint;
            World = new World(this, WorldGenerator, Path.Combine(directory, "region"));
            SaveInterval = TimeSpan.FromSeconds(5);
            saveTimer = new Timer(Save, null, (int)SaveInterval.TotalMilliseconds, Timeout.Infinite);
            tickTimer = new Timer(Tick, null, TickLength, TickLength);
        }

        private static unsafe long GenerateSeed()
        {
            double seed = MathHelper.Random.NextDouble();
            void* seedPtr = &seed;
            long Seed = *(long*)seedPtr;
            return Seed;
        }

        private void Save(object discarded)
        {
            Save();
            saveTimer = new Timer(Save, null, (int)SaveInterval.TotalMilliseconds, Timeout.Infinite);
        }

        public void Save(string directory)
        {
            LevelDirectory = directory;
            if (!Directory.Exists(LevelDirectory))
                Directory.CreateDirectory(LevelDirectory);
            if (WorldGenerator == null)
                WorldGenerator = DefaultGenerator;
            Save();
        }

        public void Save()
        {
            NbtFile file = new NbtFile();

            var serializer = new NbtSerializer(typeof(SavedLevel));
            var level = new SavedLevel
            {
                IsRaining = Raining,
                GeneratorVersion = 0,
                Time = Time,
                GameMode = (int)GameMode,
                MapFeatures = MapFeatures,
                GeneratorName = WorldGenerator.GeneratorName,
                Initialized = true,
                Seed = Seed,
                SpawnPoint = SpawnPoint,
                SizeOnDisk = 0,
                ThunderTime = ThunderTime,
                RainTime = RainTime,
                Version = 19133,
                Thundering = Thundering,
                LevelName = Name,
                LastPlayed = DateTime.UtcNow.Ticks
            };
            if (!string.IsNullOrEmpty(PlayerName))
            {
                if (File.Exists(Path.Combine(LevelDirectory, "players", PlayerName + ".dat")))
                {
                    var player = new NbtFile();
                    using (Stream stream = File.Open(Path.Combine(LevelDirectory, "players", PlayerName + ".dat"), FileMode.Open))
                        player.LoadFromStream(stream, NbtCompression.GZip, null);
                    level.Player = player.RootTag;
                    level.Player.Name = "Player";
                }
            }
            var data = serializer.Serialize(level);
            file.RootTag = new NbtCompound("");
            file.RootTag.Add(data);
            using (var stream = File.Create(Path.Combine(LevelDirectory, "level.dat")))
                file.SaveToStream(stream, NbtCompression.GZip);
            if (World.Directory == null)
                World.Save(Path.Combine(LevelDirectory, "region"));
            else
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
                file.LoadFromStream(stream, NbtCompression.None, null);
            var data = file.RootTag.Get<NbtCompound>("Data");
            var serializer = new NbtSerializer(typeof(SavedLevel));
            SavedLevel level = (SavedLevel)serializer.Deserialize(data);
            Name = level.LevelName;
            Time = level.Time;
            GameMode = (GameMode)level.GameMode;
            MapFeatures = level.MapFeatures;
            Seed = level.Seed;
            // Find world generator
            string generatorName = level.GeneratorName;
            WorldGenerator = GetGenerator(generatorName);
            WorldGenerator.Seed = Seed;
                GeneratorOptions = level.GeneratorOptions;
            WorldGenerator.Initialize(this);

            SpawnPoint = level.SpawnPoint;

            World = new World(this, WorldGenerator, Path.Combine(directory, "region"));
        }

        private void Tick(object discarded)
        {
            Time++;
        }

        public PlayerEntity LoadPlayer(string name)
        {
            PlayerEntity entity = new PlayerEntity(Difficulty);
            if (LevelDirectory == null || !File.Exists(Path.Combine(LevelDirectory, "players", name + ".dat")))
            {
                // Return default player entity
                entity.Position = SpawnPoint;
                entity.SpawnPoint = SpawnPoint + new Vector3(0, PlayerEntity.Height, 0);
                entity.Position += new Vector3(0, PlayerEntity.Height, 0);
                entity.GameMode = GameMode;
                return entity;
            }
            
            var file = new NbtFile();
            using (Stream stream = File.Open(Path.Combine(LevelDirectory, "players", name + ".dat"), FileMode.Open))
                file.LoadFromStream(stream, NbtCompression.GZip, null);
            var data = file.RootTag;
            entity.OnGround = data.Get<NbtByte>("OnGround").Value == 1;
            entity.Air = data.Get<NbtShort>("Air").Value;
            entity.Health = data.Get<NbtFloat>("HealF").Value;
            var dimension = (Dimension)data.Get<NbtInt>("Dimension").Value; // TODO
            entity.Food = (short)data.Get<NbtInt>("foodLevel").Value;
            entity.XpLevel = data.Get<NbtInt>("XpLevel").Value;
            entity.XpTotal = data.Get<NbtInt>("XpTotal").Value;
            // TODO: Set velocity based on fall distance
            entity.FoodExhaustion = data.Get<NbtFloat>("foodExhaustionLevel").Value;
            entity.FoodSaturation = data.Get<NbtFloat>("foodSaturationLevel").Value;
            entity.XpProgress = data.Get<NbtFloat>("XpP").Value;

            var equipment = data.Get<NbtList>("Equipment");
            var inventory = data.Get<NbtList>("Inventory");
            var motion = data.Get<NbtList>("Motion");
            var pos = data.Get<NbtList>("Pos");
            var rotation = data.Get<NbtList>("Rotation");
            var abilities = data.Get<NbtCompound>("abilities");

            // Appears to be unused, is overriden by the inventory contents
            // foreach (var item in equipment.Tags)

            foreach (var item in inventory)
            {
                var slot = ItemStack.FromNbt((NbtCompound)item);
                slot.Index = DataSlotToNetworkSlot(slot.Index);
                entity.Inventory[slot.Index] = slot;
            }

            entity.Velocity = new Vector3(
                ((NbtDouble)motion[0]).Value,
                ((NbtDouble)motion[1]).Value,
                ((NbtDouble)motion[2]).Value);

            entity.Position = new Vector3(
                ((NbtDouble)pos[0]).Value,
                ((NbtDouble)pos[1]).Value + PlayerEntity.Height,
                ((NbtDouble)pos[2]).Value);

            if (data.Get<NbtInt>("SpawnX") != null)
            {
                entity.SpawnPoint = new Vector3(
                    data.Get<NbtInt>("SpawnX").Value,
                    data.Get<NbtInt>("SpawnY").Value,
                    data.Get<NbtInt>("SpawnZ").Value);
            }
            else
                entity.SpawnPoint = SpawnPoint + new Vector3(0, PlayerEntity.Height, 0);

            entity.Yaw = ((NbtFloat)rotation[0]).Value;
            entity.Pitch = ((NbtFloat)rotation[1]).Value;

            // TODO: Abilities

            return entity;
        }

        public void SavePlayer(PlayerEntity entity)
        {
            // TODO: Generalize to all mobs
            NbtFile file = new NbtFile();
            var data = new NbtCompound("");
            data.Add(new NbtByte("OnGround", (byte)(entity.OnGround ? 1 : 0)));
            data.Add(new NbtShort("Air", entity.Air));
            data.Add(new NbtShort("Health", (short)entity.Health));
            data.Add(new NbtFloat("HealF", entity.Health));
            data.Add(new NbtInt("Dimension", 0)); // TODO
            data.Add(new NbtInt("foodLevel", entity.Food));
            data.Add(new NbtInt("XpLevel", entity.XpLevel));
            data.Add(new NbtInt("XpTotal", entity.XpTotal));
            data.Add(new NbtFloat("foodExhaustionLevel", entity.FoodExhaustion));
            data.Add(new NbtFloat("foodSaturationLevel", entity.FoodSaturation));
            data.Add(new NbtFloat("XpP", entity.XpProgress));
            data.Add(new NbtList("Equipment", NbtTagType.Compound));
            var inventory = new NbtList("Inventory", NbtTagType.Compound);
            for (int index = 0; index < entity.Inventory.Length; index++)
            {
                var slot = entity.Inventory[index];
                if (slot.Empty)
                    continue;
                slot.Index = NetworkSlotToDataSlot(index);
                inventory.Add(slot.ToNbt());
            }
            data.Add(inventory);
            var motion = new NbtList("Motion", NbtTagType.Double);
            motion.Add(new NbtDouble(entity.Velocity.X));
            motion.Add(new NbtDouble(entity.Velocity.Y));
            motion.Add(new NbtDouble(entity.Velocity.Z));
            data.Add(motion);

            var pos = new NbtList("Pos", NbtTagType.Double);
            pos.Add(new NbtDouble(entity.Position.X));
            pos.Add(new NbtDouble(entity.Position.Y)); 
            pos.Add(new NbtDouble(entity.Position.Z));
            data.Add(pos);

            var rotation = new NbtList("Rotation", NbtTagType.Float);
            rotation.Add(new NbtFloat(entity.Yaw));
            rotation.Add(new NbtFloat(entity.Pitch));
            data.Add(rotation);

            data.Add(new NbtCompound("abilities"));

            file.RootTag = data;
            if (!Directory.Exists(Path.Combine(LevelDirectory, "players")))
                Directory.CreateDirectory(Path.Combine(LevelDirectory, "players"));
            using (Stream stream = File.Open(Path.Combine(LevelDirectory, "players", entity.Username + ".dat"), FileMode.OpenOrCreate))
                file.SaveToStream(stream, NbtCompression.GZip);
        }

        /// <summary>
        /// Thanks to some idiot at Mojang
        /// </summary>
        private static int DataSlotToNetworkSlot(int index)
        {
            if (index <= 8)
                index += 36;
            else if (index == 100)
                index = 8;
            else if (index == 101)
                index = 7;
            else if (index == 102)
                index = 6;
            else if (index == 103)
                index = 5;
            else if (index >= 80 && index <= 83)
                index -= 79;
            return index;
        }

        private static int NetworkSlotToDataSlot(int index)
        {
            if (index >= 36 && index <= 44)
                index -= 36;
            else if (index == 8)
                index = 100;
            else if (index == 7)
                index = 101;
            else if (index == 6)
                index = 102;
            else if (index == 5)
                index = 103;
            else if (index >= 1 && index <= 4)
                index += 79;
            return index;
        }
    }
}
