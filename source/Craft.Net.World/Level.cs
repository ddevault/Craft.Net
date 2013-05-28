using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using fNbt;
using fNbt.Serialization;
using Craft.Net.Data;

namespace Craft.Net.World
{
    /// <summary>
    /// Represents a Minecraft level
    /// </summary>
    public class Level : IDisposable
    {
        [NbtIgnore]
        private IWorldGenerator WorldGenerator { get; set; }
        [NbtIgnore]
        private string DatFile { get; set; }
        [NbtIgnore]
        private string BaseDirectory { get; set; }
        [NbtIgnore]
        private List<World> Worlds { get; set; }
        [NbtIgnore]
        public World DefaultWorld
        {
            get
            {
                if (Worlds.Count == 0)
                    throw new InvalidOperationException("This level is associated with no worlds.");
                return Worlds[0];
            }
        }

        #region NBT Fields
        /// <summary>
        /// Always set to 19133.
        /// </summary>
        [TagName("version")]
        public int Version { get; private set; }
        /// <summary>
        /// True if this level has been initialized with a world.
        /// </summary>
        [TagName("initialized")]
        public bool Initialized { get; private set; }
        /// <summary>
        /// The name of this level.
        /// </summary>
        public string LevelName { get; set; }
        /// <summary>
        /// The name of the world generator to use. Corresponds to IWorldGenerator.GeneratorName.
        /// </summary>
        [TagName("generatorName")]
        public string GeneratorName { get; set; }
        /// <summary>
        /// The version number of the world generator to use.
        /// </summary>
        [TagName("generatorVersion")]
        public int GeneratorVersion { get; set; }
        /// <summary>
        /// The options to pass to the world generator.
        /// </summary>
        [TagName("generatorOptions")]
        public string GeneratorOptions { get; set; }
        /// <summary>
        /// The level's randomly generated seed.
        /// </summary>
        public long RandomSeed { get; set; }
        /// <summary>
        /// If true, structures like villages and strongholds will generate (depending on the IWorldGenerator).
        /// </summary>
        public bool MapFeatures { get; set; }
        /// <summary>
        /// Unix timestamp of last time this level was played.
        /// </summary>
        public long LastPlayed { get; set; }
        /// <summary>
        /// If true, single player users will be allowed to issue commands.
        /// </summary>
        [TagName("allowCommands")]
        public bool AllowCommands { get; set; }
        /// <summary>
        /// If true, the world is to be deleted upon the death of the user (singleplayer only).
        /// </summary>
        [TagName("hardcore")]
        public bool Hardcore { get; set; }
        private int GameType { get; set; }
        /// <summary>
        /// The default game mode for new players.
        /// </summary>
        [NbtIgnore]
        public GameMode GameMode
        {
            get { return (GameMode)GameType; }
            set { GameType = (int)value; }
        }
        /// <summary>
        /// The number of ticks since this level was created.
        /// </summary>
        public long Time { get; set; }
        /// <summary>
        /// The number of ticks in the current day.
        /// </summary>
        public long DayTime
        {
            get { return _DayTime; }
            set { _DayTime = value % 24000; }
        }
        private long _DayTime;
        /// <summary>
        /// This level's spawn point in the overworld.
        /// </summary>
        [NbtIgnore]
        public Vector3 Spawn
        {
            get
            {
                return new Vector3(SpawnX, SpawnY, SpawnZ);
            }
            set
            {
                SpawnX = (int)value.X;
                SpawnY = (int)value.Y;
                SpawnZ = (int)value.Z;
            }
        }
        private int SpawnX { get; set; }
        private int SpawnY { get; set; }
        private int SpawnZ { get; set; }
        /// <summary>
        /// True if the level is currently raining.
        /// </summary>
        [TagName("raining")]
        public bool Raining { get; set; }
        /// <summary>
        /// The number of ticks until the next weather cycle.
        /// </summary>
        [TagName("rainTime")]
        public int RainTime { get; set; }
        /// <summary>
        /// True if it is currently thundering.
        /// </summary>
        [TagName("thundering")]
        public bool Thundering { get; set; }
        /// <summary>
        /// The number of ticks until the next thunder cycle.
        /// </summary>
        [TagName("thunderTime")]
        public int ThunderTime { get; set; }
        /// <summary>
        /// The rules for this level.
        /// </summary>
        public GameRules GameRules { get; set; }
        #endregion

        /// <summary>
        /// An in-memory level, with all defaults set as such.
        /// </summary>
        public Level()
        {
            Version = 19133;
            Initialized = true;
            LevelName = "Level";
            GeneratorVersion = 1;
            GeneratorOptions = string.Empty;
            double seed = MathHelper.Random.NextDouble();
            unsafe { RandomSeed = *((long*)&seed); }
            MapFeatures = true;
            LastPlayed = DateTime.UtcNow.Ticks;
            AllowCommands = true;
            Hardcore = false;
            GameMode = GameMode.Survival;
            Time = 0;
            DayTime = 0;
            Spawn = Vector3.Zero;
            Raining = false;
            RainTime = MathHelper.Random.Next(0, 100000);
            Thundering = false;
            ThunderTime = MathHelper.Random.Next(0, 100000);
            GameRules = new GameRules();
            Worlds = new List<World>();
        }

        public Level(IWorldGenerator generator) : this()
        {
            GeneratorName = generator.GeneratorName;
            generator.Initialize(this);
            WorldGenerator = generator;
        }

        public Level(string levelName) : this()
        {
            LevelName = levelName;
        }

        public Level(IWorldGenerator generator, string levelName) : this(generator)
        {
            LevelName = levelName;
        }

        public void AddWorld(World world)
        {
            if (Worlds.Any(w => w.Name.ToUpper() == world.Name.ToUpper()))
                throw new InvalidOperationException("A world with the same name already exists in this level.");
            Worlds.Add(world);
        }

        public void AddWorld(string name)
        {
            if (Worlds.Any(w => w.Name.ToUpper() == name.ToUpper()))
                throw new InvalidOperationException("A world with the same name already exists in this level.");
            var world = new World(name);
            world.WorldGenerator = WorldGenerator;
            Worlds.Add(world);
        }

        public void SaveTo(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            Save(Path.Combine(directory, "level.dat"));
        }

        public void Save(string file)
        {
            DatFile = file;
            if (!Path.IsPathRooted(file))
                file = Path.Combine(Directory.GetCurrentDirectory(), file);
            BaseDirectory = Path.GetDirectoryName(file);
            Save();
        }

        public void Save()
        {
            if (DatFile == null)
                throw new InvalidOperationException("This level exists only in memory. Use Save(string).");
            LastPlayed = DateTime.UtcNow.Ticks;
            var serializer = new NbtSerializer(typeof(Level));
            var tag = serializer.Serialize(this, "Data") as NbtCompound;
            var file = new NbtFile();
            file.RootTag.Add(tag);
            file.SaveToFile(DatFile, NbtCompression.GZip);
            // Save worlds
            foreach (var world in Worlds)
            {
                if (world.BaseDirectory == null)
                    world.Save(Path.Combine(BaseDirectory, world.Name));
                else
                    world.Save();
            }
        }

        public static Level LoadFrom(string directory)
        {
            return Load(Path.Combine(directory, "level.dat"));
        }

        public static Level Load(string file)
        {
            if (!Path.IsPathRooted(file))
                file = Path.Combine(Directory.GetCurrentDirectory(), file);
            var serializer = new NbtSerializer(typeof(Level));
            var nbtFile = new NbtFile(file);
            var level = (Level)serializer.Deserialize(nbtFile.RootTag["Data"]);
            level.DatFile = file;
            level.BaseDirectory = Path.GetDirectoryName(file);
            var worlds = Directory.GetDirectories(level.BaseDirectory).Where(
                d => !Directory.GetFiles(d).Any(f => !f.EndsWith(".mca") && !f.EndsWith(".mcr")));
            foreach (var world in worlds)
                level.AddWorld(World.LoadWorld(world));
            return level;
        }

        public static IWorldGenerator GetGenerator(string generatorName)
        {
            IWorldGenerator worldGenerator = null;
            Type generatorType;
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes().Where(t =>
                    !t.IsAbstract && t.IsClass && typeof(IWorldGenerator).IsAssignableFrom(t) &&
                    t.GetConstructors(BindingFlags.Public).Any(c => !c.GetParameters().Any()));
                generatorType = types.FirstOrDefault(t =>
                    (worldGenerator = (IWorldGenerator)Activator.CreateInstance(t)).GeneratorName == generatorName);
                if (generatorType != null)
                    break;
            }
            return worldGenerator;
        }

        // Internally, we use network slots everywhere, but on disk, we need to use data slots
        // Maybe someday we can use the Window classes to remap these around or something
        // Or better yet, Mojang can stop making terrible design decisions
        internal static int DataSlotToNetworkSlot(int index)
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

        internal static int NetworkSlotToDataSlot(int index)
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

        public void Dispose()
        {
            foreach (var world in Worlds)
                world.Dispose();
        }
    }
}
