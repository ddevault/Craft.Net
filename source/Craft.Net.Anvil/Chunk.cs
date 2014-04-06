using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Reflection;
using fNbt;
using fNbt.Serialization;
using Craft.Net.Common;

namespace Craft.Net.Anvil
{
    public class Chunk : INbtSerializable
    {
        #region Entity Stuff

        private static NbtSerializer Serializer { get; set; }
        private static Dictionary<string, Type> EntityTypes { get; set; }

        public static void RegisterEntityType(string id, Type type)
        {
            if (typeof(IDiskEntity).IsAssignableFrom(type))
                throw new ArgumentException("Specified type does not implement IDiskEntity", "type");
            EntityTypes[id.ToUpper()] = type;
        }

        static Chunk()
        {
            EntityTypes = new Dictionary<string, Type>();
            Serializer = new NbtSerializer(typeof(Chunk));
        }

        #endregion

        public const int Width = 16, Height = 256, Depth = 16;

        [NbtIgnore]
        internal DateTime LastAccessed { get; set; }

        public bool IsModified { get; set; }

        public byte[] Biomes { get; set; }

        public int[] HeightMap { get; set; }

        [NbtIgnore]
        public Section[] Sections { get; set; }

        [TagName("TileEntities")]
        private TileEntity[] _TileEntities
        {
            get { return TileEntities.ToArray(); }
            set { TileEntities = new List<TileEntity>(value); }
        }
        [NbtIgnore]
        public List<TileEntity> TileEntities { get; set; }

        [TagName("Entities")]
        private IDiskEntity[] _Entities
        {
            get { return Entities.ToArray(); }
            set { Entities = new List<IDiskEntity>(value); }
        }
        [NbtIgnore]
        public List<IDiskEntity> Entities { get; set; }

        [TagName("xPos")]
        public int X { get; set; }

        [TagName("zPos")]
        public int Z { get; set; }

        public long LastUpdate { get; set; }

        public bool TerrainPopulated { get; set; }

        [NbtIgnore]
        public Region ParentRegion { get; set; }

        public Chunk()
        {
            TerrainPopulated = true;
            TileEntities = new List<TileEntity>();
            Entities = new List<IDiskEntity>();
            Sections = new Section[16];
            for (int i = 0; i < Sections.Length; i++)
                Sections[i] = new Section((byte)i);
            Biomes = new byte[Width * Depth];
            HeightMap = new int[Width * Depth];
            LastAccessed = DateTime.Now;
        }

        public Chunk(Coordinates2D coordinates) : this()
        {
            X = coordinates.X;
            Z = coordinates.Z;
        }

        public short GetBlockId(Coordinates3D coordinates)
        {
            LastAccessed = DateTime.Now;
            int section = GetSectionNumber(coordinates.Y);
            coordinates.Y = GetPositionInSection(coordinates.Y);
            return Sections[section].GetBlockId(coordinates);
        }

        public byte GetMetadata(Coordinates3D coordinates)
        {
            LastAccessed = DateTime.Now;
            int section = GetSectionNumber(coordinates.Y);
            coordinates.Y = GetPositionInSection(coordinates.Y);
            return Sections[section].GetMetadata(coordinates);
        }

        public byte GetSkyLight(Coordinates3D coordinates)
        {
            LastAccessed = DateTime.Now;
            int section = GetSectionNumber(coordinates.Y);
            coordinates.Y = GetPositionInSection(coordinates.Y);
            return Sections[section].GetSkyLight(coordinates);
        }

        public byte GetBlockLight(Coordinates3D coordinates)
        {
            LastAccessed = DateTime.Now;
            int section = GetSectionNumber(coordinates.Y);
            coordinates.Y = GetPositionInSection(coordinates.Y);
            return Sections[section].GetBlockLight(coordinates);
        }

        public void SetBlockId(Coordinates3D coordinates, short value)
        {
            LastAccessed = DateTime.Now;
            IsModified = true;
            int section = GetSectionNumber(coordinates.Y);
            coordinates.Y = GetPositionInSection(coordinates.Y);
            Sections[section].SetBlockId(coordinates, value);
            var oldHeight = GetHeight((byte)coordinates.X, (byte)coordinates.Z);
            if (value == 0) // Air
            {
                if (oldHeight <= coordinates.Y)
                {
                    // Shift height downwards
                    while (coordinates.Y > 0)
                    {
                        coordinates.Y--;
                        if (GetBlockId(coordinates) != 0)
                            SetHeight((byte)coordinates.X, (byte)coordinates.Z, coordinates.Y);
                    }
                }
            }
            else
            {
                if (oldHeight < coordinates.Y)
                    SetHeight((byte)coordinates.X, (byte)coordinates.Z, coordinates.Y);
            }
        }

        public void SetMetadata(Coordinates3D coordinates, byte value)
        {
            LastAccessed = DateTime.Now;
            IsModified = true;
            int section = GetSectionNumber(coordinates.Y);
            coordinates.Y = GetPositionInSection(coordinates.Y);
            Sections[section].SetMetadata(coordinates, value);
        }

        public void SetSkyLight(Coordinates3D coordinates, byte value)
        {
            LastAccessed = DateTime.Now;
            IsModified = true;
            int section = GetSectionNumber(coordinates.Y);
            coordinates.Y = GetPositionInSection(coordinates.Y);
            Sections[section].SetSkyLight(coordinates, value);
        }

        public void SetBlockLight(Coordinates3D coordinates, byte value)
        {
            LastAccessed = DateTime.Now;
            IsModified = true;
            int section = GetSectionNumber(coordinates.Y);
            coordinates.Y = GetPositionInSection(coordinates.Y);
            Sections[section].SetBlockLight(coordinates, value);
        }

        public TileEntity GetTileEntity(Coordinates3D coordinates)
        {
            LastAccessed = DateTime.Now;
            for (int i = 0; i < TileEntities.Count; i++)
                if (TileEntities[i].Coordinates == coordinates)
                    return TileEntities[i];
            return null;
        }

        public void SetTileEntity(Coordinates3D coordinates, TileEntity value)
        {
            LastAccessed = DateTime.Now;
            IsModified = true;
            for (int i = 0; i < TileEntities.Count; i++)
            {
                if (TileEntities[i].Coordinates == coordinates)
                {
                    TileEntities[i] = value;
                    return;
                }
            }
            TileEntities.Add(value);
        }

        private static int GetSectionNumber(int yPos)
        {
             return yPos / 16;
        }

        private static int GetPositionInSection(int yPos)
        {
            return yPos % 16;
        }

        /// <summary>
        /// Gets the biome at the given column.
        /// </summary>
        public Biome GetBiome(byte x, byte z)
        {
            LastAccessed = DateTime.Now;
            return (Biome)Biomes[(byte)(z * Depth) + x];
        }

        /// <summary>
        /// Sets the value of the biome at the given column.
        /// </summary>
        public void SetBiome(byte x, byte z, Biome value)
        {
            LastAccessed = DateTime.Now;
            IsModified = true;
            Biomes[(byte)(z * Depth) + x] = (byte)value;
            IsModified = true;
        }

        /// <summary>
        /// Gets the height of the specified column.
        /// </summary>
        public int GetHeight(byte x, byte z)
        {
            LastAccessed = DateTime.Now;
            return HeightMap[(byte)(z * Depth) + x];
        }

        private void SetHeight(byte x, byte z, int value)
        {
            LastAccessed = DateTime.Now;
            IsModified = true;
            HeightMap[(byte)(z * Depth) + x] = value;
        }

        public NbtFile ToNbt()
        {
            LastAccessed = DateTime.Now;
            var serializer = new NbtSerializer(typeof(Chunk));
            var compound = serializer.Serialize(this, "Level") as NbtCompound;
            var file = new NbtFile();
            file.RootTag.Add(compound);
            return file;
        }

        public static Chunk FromNbt(NbtFile nbt)
        {
            var serializer = new NbtSerializer(typeof(Chunk));
            var chunk = (Chunk)serializer.Deserialize(nbt.RootTag["Level"]);
            return chunk;
        }

        public NbtTag Serialize(string tagName)
        {
            var chunk = (NbtCompound)Serializer.Serialize(this, tagName, true);
            var entities = new NbtList("Entities", NbtTagType.Compound);
            for (int i = 0; i < Entities.Count; i++)
                entities.Add(Entities[i].Serialize(string.Empty));
            chunk.Add(entities);
            var sections = new NbtList("Sections", NbtTagType.Compound);
            var serializer = new NbtSerializer(typeof(Section));
            for (int i = 0; i < Sections.Length; i++)
            {
                if (!Sections[i].IsAir)
                    sections.Add(serializer.Serialize(Sections[i]));
            }
            chunk.Add(sections);
            return chunk;
        }

        public void Deserialize(NbtTag value)
        {
            IsModified = true;
            var compound = value as NbtCompound;
            var chunk = (Chunk)Serializer.Deserialize(value, true);

            this._TileEntities = chunk._TileEntities;
            this.Biomes = chunk.Biomes;
            this.HeightMap = chunk.HeightMap;
            this.LastUpdate = chunk.LastUpdate;
            this.Sections = chunk.Sections;
            this.TerrainPopulated = chunk.TerrainPopulated;
            this.X = chunk.X;
            this.Z = chunk.Z;

            // Entities
            var entities = compound["Entities"] as NbtList;
            Entities = new List<IDiskEntity>();
            for (int i = 0; i < entities.Count; i++)
            {
                var id = entities[i]["id"].StringValue;
                IDiskEntity entity;
                if (EntityTypes.ContainsKey(id.ToUpper()))
                    entity = (IDiskEntity)Activator.CreateInstance(EntityTypes[id]);
                else
                    entity = new UnrecognizedEntity(id);
                entity.Deserialize(entities[i]);
                Entities.Add(entity);
            }
            var serializer = new NbtSerializer(typeof(Section));
            foreach (var section in compound["Sections"] as NbtList)
            {
                int index = section["Y"].IntValue;
                Sections[index] = (Section)serializer.Deserialize(section);
                Sections[index].ProcessSection();
            }
        }
    }
}
