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

        public static World LoadWorld(string baseDirectory)
        {
            if (!Directory.Exists(baseDirectory))
                throw new DirectoryNotFoundException();
            var world = new World(Path.GetFileName(baseDirectory));
            world.BaseDirectory = baseDirectory;
            return world;
        }

        public Chunk GetChunk(Coordinates2D coordinates)
        {
            int regionX = coordinates.X / Region.Width - ((coordinates.X < 0) ? 1 : 0);
            int regionZ = coordinates.Z / Region.Depth - ((coordinates.Z < 0) ? 1 : 0);

            var region = LoadOrGenerateRegion(new Coordinates2D(regionX, regionZ));
            return region.GetChunk(new Coordinates2D(coordinates.X - regionX * 32, coordinates.Z - regionZ * 32));
        }

        public Chunk GetChunkWithoutGeneration(Coordinates2D coordinates)
        {
            int regionX = coordinates.X / Region.Width - ((coordinates.X < 0) ? 1 : 0);
            int regionZ = coordinates.Z / Region.Depth - ((coordinates.Z < 0) ? 1 : 0);

            var regionPosition = new Coordinates2D(regionX, regionZ);
            if (!Regions.ContainsKey(regionPosition)) return null;
            return Regions[regionPosition].GetChunkWithoutGeneration(
                new Coordinates2D(coordinates.X - regionX * 32, coordinates.Z - regionZ * 32));
        }

        public void SetChunk(Coordinates2D coordinates, Chunk chunk)
        {
            int regionX = coordinates.X / Region.Width - ((coordinates.X < 0) ? 1 : 0);
            int regionZ = coordinates.Z / Region.Depth - ((coordinates.Z < 0) ? 1 : 0);

            var region = LoadOrGenerateRegion(new Coordinates2D(regionX, regionZ));
            lock (region)
            {
                chunk.IsModified = true;
                region.SetChunk(new Coordinates2D(coordinates.X - regionX * 32, coordinates.Z - regionZ * 32), chunk);
            }
        }

        public void UnloadRegion(Coordinates2D coordinates)
        {
            lock (Regions)
            {
                Regions[coordinates].Save();
                Regions.Remove(coordinates);
            }
        }

        public void Save()
        {
            lock (Regions)
            {
                foreach (var region in Regions)
                    region.Value.Save();
            }
        }

        public void Save(string path)
        {
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
            BaseDirectory = path;
            lock (Regions)
            {
                foreach (var region in Regions)
                    region.Value.Save(Path.Combine(BaseDirectory, Region.GetRegionFileName(region.Key)));
            }
        }

        private Region LoadOrGenerateRegion(Coordinates2D coordinates)
        {
            if (Regions.ContainsKey(coordinates))
                return Regions[coordinates];
            var file = Path.Combine(BaseDirectory, Region.GetRegionFileName(coordinates));
            Region region;
            if (File.Exists(file))
                region = new Region(coordinates, this, file);
            else
                region = new Region(coordinates, this);
            lock (Regions)
                Regions[coordinates] = region;
            return region;
        }
        
        private Coordinates3D FindBlockPosition(Coordinates3D coordinates, out Chunk chunk)
        {
            if (coordinates.Y < 0 || coordinates.Y >= Chunk.Height)
                throw new ArgumentOutOfRangeException("coordinates", "Coordinates are out of range");

            var chunkX = coordinates.X / Chunk.Width;
            var chunkZ = coordinates.Z / Chunk.Depth;

            chunk = GetChunk(new Coordinates2D(chunkX, chunkZ));
            return new Coordinates3D(
                (coordinates.X - chunkX * Chunk.Width) % Chunk.Width,
                coordinates.Y,
                (coordinates.Z - chunkZ * Chunk.Depth) % Chunk.Depth);
        }
    }
}