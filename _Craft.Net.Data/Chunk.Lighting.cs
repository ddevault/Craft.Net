using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data
{
    public partial class Chunk
    {
        public void ClearLight()
        {
            foreach (var section in Sections)
            {
                Array.Clear(section.BlockLight.Data, 0, section.BlockLight.Data.Length);
                Array.Clear(section.SkyLight.Data, 0, section.SkyLight.Data.Length);
            }
        }

        public void CalculateInitialSkylight()
        {
            // First pass, sets initial values
            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Depth; z++)
                {
                    SkyLightColumn(x, z);
                }
            }
        }

        private void SkyLightColumn(int x, int z)
        {
            byte light = 15;
            for (int y = Height - 1; y >= 0; y--)
            {
                var id = GetBlockId(x, y, z);
                if (id < World.LightReductionIndex.Length)
                    light -= World.LightReductionIndex[id];
                if (light == 0)
                    break;
                SetSkyLight(x, y, z, light);
            }
        }
    }
}
