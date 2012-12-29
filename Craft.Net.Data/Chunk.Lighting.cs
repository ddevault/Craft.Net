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
                    if (x == 0 || z == 0 || x == Width - 1 || z == Depth - 1)
                        AdjustSkyLightEdgeColumn(x, z);
                    else
                        AdjustSkyLightColumn(x, z);
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

        private void AdjustSkyLightEdgeColumn(int x, int z)
        {
            bool checkLeft = false, checkRight = false, checkForward = false,
                checkBackward = false;
            if (x == 0 && ParentRegion.Chunks.ContainsKey(RelativePosition + Vector3.Left))
                checkLeft = true;
            if (x == Width - 1 && ParentRegion.Chunks.ContainsKey(RelativePosition + Vector3.Right))
                checkRight = true;
            if (z == 0 && ParentRegion.Chunks.ContainsKey(RelativePosition + Vector3.Backwards))
                checkBackward = true;
            if (z == Depth - 1 && ParentRegion.Chunks.ContainsKey(RelativePosition + Vector3.Forwards))
                checkForward = true;
            for (int y = 0; y < Height; y++)
            {
                byte self = GetSkyLight(x, y, z);
                if (self < 2)
                    continue;
                byte left = GetSkyLight(x - 1, y, z);
                byte right = GetSkyLight(x + 1, y, z);
                byte forward = GetSkyLight(x, y, z + 1);
                byte backward = GetSkyLight(x, y, z - 1);
                if (left == 15 && right == 15 && forward == 15 && backward == 15)
                    break;
                if (left < self) SetSkyLight(x - 1, y, z, (byte)(self - 1));
                if (right < self) SetSkyLight(x + 1, y, z, (byte)(self - 1));
                if (forward < self) SetSkyLight(x, y, z + 1, (byte)(self - 1));
                if (backward < self) SetSkyLight(x, y, z - 1, (byte)(self - 1));
            }
        }

        private void AdjustSkyLightColumn(int x, int z)
        {
            for (int y = 0; y < Height; y++)
            {
                byte self = GetSkyLight(x, y, z);
                if (self < 2)
                    continue;
                byte left = GetSkyLight(x - 1, y, z);
                byte right = GetSkyLight(x + 1, y, z);
                byte forward = GetSkyLight(x, y, z + 1);
                byte backward = GetSkyLight(x, y, z - 1);
                if (left == 15 && right == 15 && forward == 15 && backward == 15)
                    break;
                if (left < self)     SetSkyLight(x - 1, y, z, (byte)(self - 1));
                if (right < self)    SetSkyLight(x + 1, y, z, (byte)(self - 1));
                if (forward < self)  SetSkyLight(x, y, z + 1, (byte)(self - 1));
                if (backward < self) SetSkyLight(x, y, z - 1, (byte)(self - 1));
            }
        }
    }
}
