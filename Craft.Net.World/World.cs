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
        public Dictionary<Coordinates2D, Region> Regions { get; set; }
        public IWorldGenerator WorldGenerator { get; set; }

        public World(string name)
        {
            Name = name;
            Regions = new Dictionary<Coordinates2D, Region>();
        }

        public World(string name, IWorldGenerator worldGenerator) : this(name)
        {
            WorldGenerator = worldGenerator;
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

        /// <summary>
        /// Gets the region at the given region coordinates.
        /// </summary>
        /// <param name="coordinates">The region to retrieve, in region coordinates</param>
        public Region GetRegion(Coordinates2D coordinates)
        {
            if (Regions.ContainsKey(coordinates))
                return Regions[coordinates];
            if (WorldGenerator != null)
                return LoadOrGenerateRegion(coordinates);
            throw new KeyNotFoundException("The requested region is not present in this world.");
        }

        private Region LoadOrGenerateRegion(Coordinates2D coordinates)
        {
            var file = Path.Combine(BaseDirectory, Region.GetRegionFileName(coordinates));
            Region region;
            if (File.Exists(file))
                region = new Region(coordinates, this, file);
            else
                region = new Region(coordinates, this);
            Regions[coordinates] = region;
            return region;
        }

        public short GetBlockId(

        #region Coordinate Conversion

        

        #endregion
    }
}