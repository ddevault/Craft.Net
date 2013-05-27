using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;

namespace Craft.Net.World
{
    public class World
    {
        public const int Height = 256;

        public string Name { get; set; }
        public string BaseDirectory { get; internal set; }
        public Dictionary<Coordinates, Region> Regions { get; set; }
        public IWorldGenerator WorldGenerator { get; set; }

        public World(string name)
        {
            Name = name;
            Regions = new Dictionary<Coordinates, Region>();
        }

        /// <summary>
        /// Loads a world from region files on disk.
        /// </summary>
        /// <param name="baseDirectory">The path to the base directory, with region files corresponding to the regions
        /// of the world to load.</param>
        public static World LoadWorld(string baseDirectory)
        {
            if (!Directory.Exists(baseDirectory))
                throw new DirectoryNotFoundException();
            var world = new World(Path.GetFileName(baseDirectory));
            world.BaseDirectory = baseDirectory;
            return world;
        }
    }
}