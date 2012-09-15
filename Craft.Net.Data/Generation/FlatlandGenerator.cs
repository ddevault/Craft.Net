
using System;
using System.Collections.Generic;
using Craft.Net.Data.Blocks;
namespace Craft.Net.Data.Generation
{
    /// <summary>
    /// Generates a world that mimics the Minecraft flatland generator
    /// with structures turned off.
    /// </summary>
    public class FlatlandGenerator : IWorldGenerator
    {
        static FlatlandGenerator()
        {
            DefaultGeneratorOptions = "1;7,2x3,2;1";
        }

        public static string DefaultGeneratorOptions { get; set; }

        public string GeneratorOptions { get; set; }
        public Biome Biome { get; set; }

        protected List<GeneratorLayer> Layers;

        public void Initialize(Level level)
        {
            if (level != null)
                GeneratorOptions = level.GeneratorOptions ?? DefaultGeneratorOptions;
            else
                GeneratorOptions = DefaultGeneratorOptions;
            if (string.IsNullOrEmpty(GeneratorOptions))
                GeneratorOptions = DefaultGeneratorOptions;
            var parts = GeneratorOptions.Split(';');
            var layers = parts[1].Split(',');
            Layers = new List<GeneratorLayer>();
            foreach (var layer in layers)
                Layers.Add(new GeneratorLayer(layer));
            Biome = (Biome)byte.Parse(parts[2]);
        }

        public Chunk GenerateChunk(Vector3 position, Region parentRegion)
        {
            var chunk = GenerateChunk(position);
            chunk.ParentRegion = parentRegion;
            return chunk;
        }

        public Chunk GenerateChunk(Vector3 position)
        {
            var chunk = new Chunk(position);
            int y = 0;
            for (int i = 0; i < Layers.Count; i++)
            {
                int height = y + Layers[i].Height;
                while (y < height)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        for (int z = 0; z < 16; z++)
                        {
                            chunk.SetBlock(new Vector3(x, y, z), Layers[i].Block);
                        }
                    }
                    y++;
                }
            }
            for (int i = 0; i < chunk.Biomes.Length; i++)
                chunk.Biomes[i] = (byte)Biome;
            return chunk;
        }

        public string LevelType
        {
            get { return "FLAT"; }
        }

        public string GeneratorName { get { return "FLAT"; } }

        public long Seed { get; set; }

        public Vector3 SpawnPoint
        {
            get { return new Vector3(0, 4, 0); }
        }

        protected class GeneratorLayer
        {
            public GeneratorLayer(string layer)
            {
                var parts = layer.Split('x');
                int idIndex = 0;
                if (parts.Length == 2)
                    idIndex++;
                var idParts = parts[idIndex].Split(':');
                BlockId = ushort.Parse(idParts[0]);
                if (idParts.Length == 2)
                    Metadata = (byte)(byte.Parse(idParts[1]) & 0xF);
                Height = 1;
                if (parts.Length == 2)
                    Height = int.Parse(parts[0]);
            }

            public Block Block
            {
                get
                {
                    Block b = (Block)BlockId;
                    b.Metadata = Metadata;
                    return b;
                }
            }

            public ushort BlockId { get; set; }
            public byte Metadata { get; set; }
            public int Height { get; set; }
        }
    }
}