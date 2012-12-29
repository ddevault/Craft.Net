using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data
{
    public partial class Chunk
    {
        public static void RecreateLightIndex()
        {
            lightReductionIndex = new byte[0];
            foreach (var item in Item.Items)
            {
                if (item is Block)
                {
                    if (item.Id >= lightReductionIndex.Length)
                        Array.Resize(ref lightReductionIndex, item.Id + 1);
                    lightReductionIndex[item.Id] = (item as Block).LightReduction;
                }
            }
        }

        private static byte[] lightReductionIndex;

        public void Light()
        {
            foreach (var section in Sections)
            {
                Array.Clear(section.BlockLight.Data, 0, section.BlockLight.Data.Length);
                Array.Clear(section.SkyLight.Data, 0, section.SkyLight.Data.Length);
            }
            CalculateSkylight();
        }

        public void CalculateSkylight()
        {
            // First pass, sets initial values
            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Depth; z++)
                {
                    SkyLightColumn(x, z);
                }
            }
            // Second pass, adjusts values based on neighboring blocks
            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Depth; z++)
                {
                    
                }
            }
        }

        private void SkyLightColumn(int x, int z)
        {
            byte light = 15;
            for (int y = Height - 1; y >= 0; y--)
            {
                var id = GetBlockId(x, y, z);
                if (id < lightReductionIndex.Length)
                    light -= lightReductionIndex[id];
                if (light == 0)
                    break;
                SetSkyLight(x, y, z, light);
            }
        }

        /// <summary>
        /// Adjusts this block's sky light based on its neighbors,
        /// and updates neighbors if needed.
        /// May propegate into pre-generated neighboring chunks.
        /// </summary>
        public void AdjustSkyLight(int x, int y, int z)
        {
            byte self = GetSkyLight(x, y, z);
            byte left = GetAdjacentSkyLight(x - 1, y, z);
            byte right = GetAdjacentSkyLight(x + 1, y, z);
            byte backwards = GetAdjacentSkyLight(x, y, z - 1);
            byte forwards = GetAdjacentSkyLight(x, y, z + 1);
        }

        private byte GetAdjacentSkyLight(int x, int y, int z)
        {
            // Gets a neighboring block's sky light, accounting
            // for overflow/underflow with other chunks, and
            // the generated state of those chunks.
            // Returns 15 (max value) for adjacent, ungenerated
            // chunks.
            return 15; // TODO
        }
    }
}
