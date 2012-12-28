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
    }
}
