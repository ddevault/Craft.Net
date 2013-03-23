using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Craft.Net.Data;

namespace Craft.Net.Client
{
    public class ReadOnlyWorld
    {
        private bool UnloadChunks { get; set; }

        internal World World { get; set; }
        internal Level Level { get; set; }

        internal ReadOnlyWorld()
        {
            World = new World(null);
            Level = new Level(World);
            World.EnableBlockUpdates = false;
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

        public Block GetBlock(Vector3 position)
        {
            return World.GetBlock(position);
        }

        public ReadOnlyChunk GetChunk(Vector3 position)
        {
            return new ReadOnlyChunk(World.GetChunk(position));
        }

        internal void SetChunk(Vector3 position, Chunk chunk)
        {
            World.SetChunk(position, chunk);
        }

        internal void RemoveChunk(int x, int z)
        {
            if (UnloadChunks)
                World.UnloadChunk(x, z);
        }
    }

    public class ReadOnlyChunk
    {
        internal Chunk Chunk { get; set; }

        internal ReadOnlyChunk(Chunk chunk)
        {
            Chunk = chunk;
        }

        public Block GetBlock(Vector3 position)
        {
            return Chunk.GetBlock(position);
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

        public Block GetBlock(Vector3 position)
        {
            return Section.GetBlock(position);
        }

        public short GetBlockId(int x, int y, int z)
        {
            return Section.GetBlockId(x, y, z);
        }

        public byte GetBlockLight(int x, int y, int z)
        {
            return Section.GetBlockLight(x, y, z);
        }

        public byte GetSkyLight(int x, int y, int z)
        {
            return Section.GetSkyLight(x, y, z);
        }

        public ReadOnlyCollection<byte> Blocks { get { return Array.AsReadOnly(Section.Blocks); } }
        public ReadOnlyNibbleArray Metadata { get { return new ReadOnlyNibbleArray(Section.Metadata); } }
        public ReadOnlyNibbleArray BlockLight { get { return new ReadOnlyNibbleArray(Section.BlockLight); } }
        public ReadOnlyNibbleArray SkyLight { get { return new ReadOnlyNibbleArray(Section.SkyLight); } }
    }
}
