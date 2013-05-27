using Craft.Net.Data;
using fNbt.Serialization;

namespace Craft.Net.World
{
    public class Section
    {
        public const byte Width = 16, Height = 16, Depth = 16;

        public byte[] Blocks { get; set; }
        [TagName("Data")]
        public NibbleArray Metadata { get; set; }
        public NibbleArray BlockLight { get; set; }
        public NibbleArray SkyLight { get; set; }
        [IgnoreOnNull]
        public NibbleArray Add { get; set; }
        public byte Y { get; set; }

        private int nonAirCount;

        public Section(byte y)
        {
            const int size = Width * Height * Depth;
            this.Y = y;
            Blocks = new byte[size];
            Metadata = new NibbleArray(size);
            BlockLight = new NibbleArray(size);
            SkyLight = new NibbleArray(size);
            Add = null; // Only used when needed
            nonAirCount = 0;
        }

        public bool IsAir
        {
            get { return nonAirCount == 0; }
        }

        public short GetBlockId(int x, int y, int z)
        {
            int index = x + (z * Width) + (y * Height * Width);
            return Blocks[index];
        }

        public byte GetSkyLight(int x, int y, int z)
        {
            int index = x + (z * Width) + (y * Height * Width);
            return SkyLight[index];
        }

        public byte GetBlockLight(int x, int y, int z)
        {
            int index = x + (z * Width) + (y * Height * Width);
            return BlockLight[index];
        }

        public void SetSkyLight(int x, int y, int z, byte value)
        {
            int index = x + (z * Width) + (y * Height * Width);
            SkyLight[index] = value;
        }

        public void SetBlockLight(int x, int y, int z, byte value)
        {
            int index = x + (z * Width) + (y * Height * Width);
            BlockLight[index] = value;
        }

        public void ProcessSection()
        {
            // TODO: Schedule updates
            nonAirCount = 0;
            for (int i = 0; i < Blocks.Length; i++)
            {
                if (Blocks[i] != 0)
                    nonAirCount++;
            }
        }
    }
}