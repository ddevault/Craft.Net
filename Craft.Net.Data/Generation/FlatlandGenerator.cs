
using System;
using System.Collections.Generic;
using Craft.Net.Data.Blocks;
namespace Craft.Net.Data.Generation
{
    /// <summary>
    /// Generates a world that mimics the Minecraft flatland generator
    /// with structures turned on.
    /// added a chance to generate villages.
    /// villages are generated from a text file (village.txt),like this x,y,z,type:x,y,z,type:x,y,z,type etc..
    /// test village is just two squares (one stone one wooden planks) on top of each other
    /// </summary>
    public class FlatlandGenerator : IWorldGenerator
    {
        static FlatlandGenerator()
        {
            DefaultGeneratorOptions = "1;7,2x3,2;1";
        }

        public FlatlandGenerator()
        {
            SpawnPoint = new Vector3(0, 4, 0);
        }

        public FlatlandGenerator(string generatorOptions)
        {
            GeneratorOptions = generatorOptions;
        }

        public static string DefaultGeneratorOptions { get; set; }

        public string GeneratorOptions
        {
            get { return generatorOptions; }
            set
            {
                generatorOptions = value;
                CreateLayers();
            }
        }

        public Biome Biome { get; set; }

        protected List<GeneratorLayer> Layers;
        private string generatorOptions;

        public void Initialize(Level level)
        {
            if (GeneratorOptions != null)
                return;
            if (level != null)
                GeneratorOptions = level.GeneratorOptions ?? DefaultGeneratorOptions;
            else
                GeneratorOptions = DefaultGeneratorOptions;
            if (string.IsNullOrEmpty(GeneratorOptions))
                GeneratorOptions = DefaultGeneratorOptions;
        }

        private void CreateLayers()
        {
            var parts = GeneratorOptions.Split(';');
            var layers = parts[1].Split(',');
            Layers = new List<GeneratorLayer>();
            double y = 0;
            foreach (var layer in layers)
            {
                var generatorLayer = new GeneratorLayer(layer);
                y += generatorLayer.Height;
                Layers.Add(generatorLayer);
            }
            Biome = (Biome)byte.Parse(parts[2]);
            SpawnPoint = new Vector3(0, y, 0);
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
            int r = new Random().Next(1, 5);
            if (r < 2)
            {
                buildVillage(chunk);
            }
            for (int i = 0; i < chunk.Biomes.Length; i++)
                chunk.Biomes[i] = (byte)Biome;
            return chunk;
        }

        public void buildVillage(Chunk c)
        {
            int t = 0;
            foreach (GeneratorLayer layer in Layers)
            {
                t += layer.Height;
            }

            System.IO.StreamReader s = new System.IO.StreamReader("village.txt");
            String[] p = s.ReadToEnd().Split(":".ToCharArray());
            s.Close();
            foreach (string tr in p)
            {
                String[] plan = tr.Split(",".ToCharArray());
                switch (plan[3])
                {
                    case "air":
                        c.SetBlock(new Vector3(double.Parse(plan[0]),double.Parse(t.ToString()) + double.Parse(plan[1]),double.Parse(plan[2])), new AirBlock() );
                        break;
                    case "stone":
                        c.SetBlock(new Vector3(double.Parse(plan[0]), double.Parse(t.ToString()) + double.Parse(plan[1]), double.Parse(plan[2])), new StoneBlock());
                        break;
                    case "wood":
                        c.SetBlock(new Vector3(double.Parse(plan[0]), double.Parse(t.ToString()) + double.Parse(plan[1]), double.Parse(plan[2])), new WoodBlock());
                        break;
                    case "woodenplanks":
                        c.SetBlock(new Vector3(double.Parse(plan[0]), double.Parse(t.ToString()) + double.Parse(plan[1]), double.Parse(plan[2])), new WoodenPlanksBlock());
                        break;
                    case "stonebrick":
                        c.SetBlock(new Vector3(double.Parse(plan[0]), double.Parse(t.ToString()) + double.Parse(plan[1]), double.Parse(plan[2])), new StoneBrickBlock());
                        break;
                    case "glass":
                        c.SetBlock(new Vector3(double.Parse(plan[0]), double.Parse(t.ToString()) + double.Parse(plan[1]), double.Parse(plan[2])), new GlassPaneBlock());
                        break;
                }
            }

        }

        public string LevelType
        {
            get { return "FLAT"; }
        }

        public string GeneratorName { get { return "FLAT"; } }

        public long Seed { get; set; }

        public Vector3 SpawnPoint { get; set; }

        protected class GeneratorLayer
        {
            public GeneratorLayer(string layer)
            {
                var parts = layer.Split('x');
                int idIndex = 0;
                if (parts.Length == 2)
                    idIndex++;
                var idParts = parts[idIndex].Split(':');
                BlockId = short.Parse(idParts[0]);
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

            public short BlockId { get; set; }
            public byte Metadata { get; set; }
            public int Height { get; set; }
        }
    }
}