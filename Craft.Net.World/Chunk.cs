using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Reflection;
using fNbt;
using fNbt.Serialization;

namespace Craft.Net.World
{
    public class Chunk
    {
        public const int Width = 16, Height = 256, Depth = 16;

        internal bool IsModified { get; set; }

        public byte[] Biomes { get; set; }

        public int[] HeightMap { get; set; }

        public Section[] Sections { get; set; }

        [TagName("xPos")]
        public int X { get; set; }

        [TagName("yPos")]
        public int Y { get; set; }

        public long LastUpdate { get; set; }

        private bool TerrainPopulated { get; set; }

        public Chunk()
        {
            TerrainPopulated = true;
        }

        public short GetBlockId(int x, int y, int z)
        {
            int section = GetSectionNumber(y);
            y = GetPositionInSection(y);
            return Sections[section].GetBlockId(x, y, z);
        }

        public byte GetSkyLight(int x, int y, int z)
        {
            int section = GetSectionNumber(y);
            y = GetPositionInSection(y);
            return Sections[section].GetSkyLight(x, y, z);
        }

        public byte GetBlockLight(int x, int y, int z)
        {
            int section = GetSectionNumber(y);
            y = GetPositionInSection(y);
            return Sections[section].GetBlockLight(x, y, z);
        }

        public void SetSkyLight(int x, int y, int z, byte value)
        {
            int section = GetSectionNumber(y);
            y = GetPositionInSection(y);
            Sections[section].SetSkyLight(x, y, z, value);
        }

        public void SetBlockLight(int x, int y, int z, byte value)
        {
            int section = GetSectionNumber(y);
            y = GetPositionInSection(y);
            Sections[section].SetBlockLight(x, y, z, value);
        }

        private static int GetSectionNumber(double yPos)
        {
             return (int)(yPos / 16);
        }

        private static int GetPositionInSection (double yPos)
        {
            return (int)yPos % 16;
        }

        /// <summary>
        /// Gets the biome at the given column.
        /// </summary>
        public Biome GetBiome(byte x, byte z)
        {
            return (Biome)Biomes[(byte)(z * Depth) + x];
        }

        /// <summary>
        /// Sets the value of the biome at the given column.
        /// </summary>
        public void SetBiome(byte x, byte z, Biome value)
        {
            Biomes[(byte)(z * Depth) + x] = (byte)value;
            IsModified = true;
        }

        /// <summary>
        /// Gets the height of the specified column.
        /// </summary>
        public int GetHeight(byte x, byte z)
        {
            return HeightMap[(byte)(z * Depth) + x];
        }
    }
}
