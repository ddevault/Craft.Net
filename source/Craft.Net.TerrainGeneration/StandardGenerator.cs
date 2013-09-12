
using System;
using Craft.Net.Anvil;
using Craft.Net.Common;
using System.Collections.Generic;
using System.Collections;

namespace Craft.Net.TerrainGeneration
{
    public class StandardGenerator : IWorldGenerator
    {
        public string LevelType { get { return "STANDARD"; } }
        public string GeneratorName { get { return "STANDARD"; } }
        public string GeneratorOptions { get; set; }
        public long Seed { get; set; }
        public Vector3 SpawnPoint { get; set; }

		private Noise noise;

		private int waterLevel = 20;



        public Chunk GenerateChunk(Coordinates2D position)
		{
			int trees = new Random ().Next (0, 4);
			int[,] heights = new int[16,16];
			int[,] treeBasePositions = new int[trees,2];


			for (int t = 0; t < trees; t++) {
				treeBasePositions [t, 0] = new Random ().Next (1, 16);
				treeBasePositions [t, 1] = new Random ().Next (1, 16);
			}


            //Make a new Chunk
            Chunk c = new Chunk(position);
            //Loop through all the blocks in the chunk
            for (int x = 0; x < 16; x++)
            {
                for (int z = 0; z < 16; z++)
                {
					int h = -1 * (int)noise.Get2D (c.X * 16 + x, c.Z * 16 + z);
					if (h <= 0)
						h = h * -1 + 4;
                    for (int y = 0; y < h; y++)
                    {

						if (y == 0)
                            //if at the bottom then set block to bedrock
							c.SetBlockId (new Coordinates3D (x, y, z), 7);
						else if (y < h - 1)
                            //if not at the top set the block to dirt
							c.SetBlockId (new Coordinates3D (x, y, z), 3);
						else if (y < waterLevel)
							//if below the water set to sand or clay
						if (new Random ().Next (1, 40) < 5 && y < waterLevel - 4)
							c.SetBlockId (new Coordinates3D (x, y, z), 82);
						else
							c.SetBlockId (new Coordinates3D (x, y, z), 12);
						else
                            //otherwise set the block to grass or gravel rarely
							if (new Random ().Next (1, 200) == 1)
							c.SetBlockId (new Coordinates3D (x, y, z), 13);
						else
							c.SetBlockId (new Coordinates3D (x, y, z), 2);


                    }

					heights [x,z] = h;


					//create beaches and place water
					if (h <= waterLevel) {
						for (int w = 0; w < waterLevel - 3; w++) {
							if (c.GetBlockId(new Coordinates3D(x,w,z)) == 0){
								c.SetBlockId(new Coordinates3D(x,w,z), 8);
							}
						}
					}



					//generate trees
					for (int pos = 0; pos < trees; pos++) {
						int random = new Random().Next (3, 4);
						int height = heights[treeBasePositions [pos, 0] , treeBasePositions [pos, 1]];
						if (height < waterLevel + 10)
							break;
						for (int t = 0; t <= random; t++) {
							c.SetBlockId (new Coordinates3D (treeBasePositions [pos, 0], height + t, treeBasePositions [pos, 1]), 17);
						}

						int leafwidth = 4;
						for (int layer = 0; layer <= height; layer++) {
							for (int w = 0; w <= leafwidth; w++) {
								for (int l = 0; l <= leafwidth; l++) {
									c.SetBlockId (new Coordinates3D (treeBasePositions[pos,0] - (leafwidth/2) + w, height + layer + random,treeBasePositions[pos,1] - (leafwidth/2) + l), 18);
								}							}
								leafwidth -= 1;
						}

					}


                }
            }




            return c;
        }

        /// <summary>
        /// Called after the world generator is created and
        /// all values are set.
        /// </summary>
        public void Initialize(Level level)
        {
            SpawnPoint = new Vector3(0, 60, 0);
			if(Seed.Equals(new long()))
				noise = new Noise (1, 0.01, 156, 2, new Random(new Random(new Random().Next()).Next()).Next());
			else
				noise = new Noise (1, 0.02, 156, 2, (int)Seed);

        }
	

		private int RandRange(Random r, int rMin, int rMax)
		{
			return rMin + r.Next() * (rMax - rMin);
		}

		private double RandRange(Random r, double rMin, double rMax)
		{
			return rMin + r.NextDouble() * (rMax - rMin);
		}

		private float RandRange(Random r, float rMin, float rMax)
		{
			return rMin + (float)r.NextDouble() * (rMax - rMin);
		}

		// Returns true if a is a power of 2, else false
		private bool pow2(int a)
		{
			return (a & (a - 1)) == 0;
		}

	
    }
}

