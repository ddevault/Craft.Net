using Craft.Net.Server.Blocks;

namespace Craft.Net.Server.Worlds
{
    public class Chunk
    {
        public const int Width = 16, Height = 256, Depth = 16;

        public Vector3 AbsolutePosition;
        public byte[] Biomes;
        public Region ParentRegion;
        public Vector3 RelativePosition;
        public Section[] Sections;

        public Chunk(Vector3 relativePosition, Region parentRegion)
        {
            Sections = new Section[16];
            for (int i = 0; i < Sections.Length; i++)
                Sections[i] = new Section((byte)i);
            this.RelativePosition = relativePosition;
            Biomes = new byte[Width*Depth];
            this.ParentRegion = parentRegion;
            AbsolutePosition =
                (parentRegion.Position*new Vector3(Region.Width, 0, Region.Depth))
                + relativePosition;
        }

        public Chunk(Vector3 relativePosition)
        {
            Sections = new Section[16];
            for (int i = 0; i < Sections.Length; i++)
                Sections[i] = new Section((byte)i);
            this.RelativePosition = relativePosition;
            Biomes = new byte[Width*Depth];
        }

        /// <summary>
        /// Sets the value of the block at the given position, relative to this chunk.
        /// </summary>
        public void SetBlock(Vector3 position, Block value)
        {
            var y = (byte)position.Y;
            y /= 16;
            position.Y = position.Y%16;
            Sections[y].SetBlock(position, value);
        }

        public Block GetBlock(Vector3 position)
        {
            var y = (byte)position.Y;
            y /= 16;
            position.Y = position.Y%16;
            return Sections[y].GetBlock(position);
        }
    }
}