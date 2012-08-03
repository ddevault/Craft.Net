using System;
using Craft.Net.Server.Blocks;

namespace Craft.Net.Server.Worlds
{
    public class Chunk
    {
        public const int Width = 16, Height = 256, Depth = 16;

        public Section[] Sections;
        public byte[] Biomes;
        public Vector3 RelativePosition, AbsolutePosition;
        public Region ParentRegion;

        public Chunk(Vector3 RelativePosition, Region ParentRegion)
        {
            Sections = new Section[16];
            for (int i = 0; i < Sections.Length; i++)
                Sections[i] = new Section((byte)i);
            this.RelativePosition = RelativePosition;
            this.Biomes = new byte[Width * Depth];
            this.ParentRegion = ParentRegion;
            this.AbsolutePosition = 
                (ParentRegion.Position * new Vector3(Region.Width, 0, Region.Depth)) 
                + RelativePosition;
        }

        /// <summary>
        /// Sets the value of the block at the given position, relative to this chunk.
        /// </summary>
        public void SetBlock(Vector3 position, Block value)
        {
            byte y = (byte)position.Y;
            y /= 16;
            position.Y = position.Y % 16;
            Sections[y].SetBlock(position, value);
        }

        public Block GetBlock(Vector3 position)
        {
            byte y = (byte)position.Y;
            y /= 16;
            position.Y = position.Y % 16;
            return Sections[y].GetBlock(position);
        }
    }
}

