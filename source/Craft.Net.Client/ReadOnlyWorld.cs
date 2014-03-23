using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Craft.Net.Anvil;
using Craft.Net.Common;

namespace Craft.Net.Client
{
    public class ReadOnlyWorld
    {
        private bool UnloadChunks { get; set; }

        internal World World { get; set; }
        internal Level Level { get; set; }

        internal ReadOnlyWorld()
        {
            World = new World("default");
            Level = new Level();
            Level.AddWorld(World);
            UnloadChunks = true;
        }

        /// <summary>
        /// Saves the world to a directory and enables saving from then on.
        /// </summary>
        public World Save(string path, bool disableChunkUnloading)
        {
            Level.Save(path);
            UnloadChunks = !disableChunkUnloading;
            return World;
        }

        public short GetBlockId(Coordinates3D coordinates)
        {
            return World.GetBlockId(coordinates);
        }

        internal void SetBlockId(Coordinates3D coordinates, short value) {
          World.SetBlockId(coordinates, value);
        }

        internal void SetMetadata(Coordinates3D coordinates, byte value) {
          World.SetMetadata(coordinates, value);
        }

        public byte GetMetadata(Coordinates3D coordinates)
        {
            return World.GetMetadata(coordinates);
        }

        public byte GetSkyLight(Coordinates3D coordinates)
        {
            return World.GetSkyLight(coordinates);
        }

        public byte GetBlockLight(Coordinates3D coordinates)
        {
            return World.GetBlockLight(coordinates);
        }

        public ReadOnlyChunk GetChunk(Coordinates2D coordinates)
        {
            return new ReadOnlyChunk(World.GetChunk(coordinates));
        }

        internal void SetChunk(Coordinates2D coordinates, Chunk chunk)
        {
            World.SetChunk(coordinates, chunk);
        }

        internal void RemoveChunk(Coordinates2D coordinates)
        {
            if (UnloadChunks)
                World.UnloadChunk(coordinates);
        }
    }

    public class ReadOnlyChunk
    {
        internal Chunk Chunk { get; set; }

        internal ReadOnlyChunk(Chunk chunk)
        {
            Chunk = chunk;
        }

        public short GetBlockId(Coordinates3D coordinates)
        {
            return Chunk.GetBlockId(coordinates);
        }

        public byte GetMetadata(Coordinates3D coordinates)
        {
            return Chunk.GetMetadata(coordinates);
        }

        public byte GetSkyLight(Coordinates3D coordinates)
        {
            return Chunk.GetSkyLight(coordinates);
        }

        public byte GetBlockLight(Coordinates3D coordinates)
        {
            return Chunk.GetBlockLight(coordinates);
        }

        public Biome GetBiome(byte x, byte z)
        {
            return Chunk.GetBiome(x, z);
        }

        public ReadOnlySection GetSection(byte index)
        {
            return new ReadOnlySection(Chunk.Sections[index]);
        }
    }

    public class ReadOnlySection
    {
        internal Section Section { get; set; }

        internal ReadOnlySection(Section section)
        {
            Section = section;
        }

        public short GetBlockId(Coordinates3D coordinates)
        {
            return Section.GetBlockId(coordinates);
        }

        public byte GetMetadata(Coordinates3D coordinates)
        {
            return Section.GetMetadata(coordinates);
        }

        public byte GetSkyLight(Coordinates3D coordinates)
        {
            return Section.GetSkyLight(coordinates);
        }

        public byte GetBlockLight(Coordinates3D coordinates)
        {
            return Section.GetBlockLight(coordinates);
        }

        public ReadOnlyCollection<byte> Blocks { get { return Array.AsReadOnly(Section.Blocks); } }
        public ReadOnlyNibbleArray Metadata { get { return new ReadOnlyNibbleArray(Section.Metadata); } }
        public ReadOnlyNibbleArray BlockLight { get { return new ReadOnlyNibbleArray(Section.BlockLight); } }
        public ReadOnlyNibbleArray SkyLight { get { return new ReadOnlyNibbleArray(Section.SkyLight); } }
    }
}
