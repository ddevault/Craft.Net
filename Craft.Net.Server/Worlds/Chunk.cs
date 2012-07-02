using System;
using Craft.Net.Server.Blocks;

namespace Craft.Net.Server.Worlds
{
    public class Chunk
    {
        public Section[] Sections;

        public Chunk()
        {
            Sections = new Section[16];
        }

        /// <summary>
        /// Sets the value of the block at the given position, relative to this chunk.
        /// </summary>
        public void SetBlock(Vector3 position, Block value)
        {
            byte y = (byte)position.Y;
            y /= 16;
            position.Y = y % 16;
            Sections[y].SetBlock(position, value);
        }

        public void SetBlock(Vector3 position, byte value)
        {
            byte y = (byte)position.Y;
            y /= 16;
            position.Y = y % 16;
            Sections[y].SetBlock(position, value);
        }
    }
}

