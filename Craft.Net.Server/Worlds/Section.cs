using Craft.Net.Server.Blocks;
using ICSharpCode.SharpZipLib.Zip.Compression;

namespace Craft.Net.Server.Worlds
{
    public class Section
    {
        public const byte Width = 16, Height = 16, Depth = 16;
        public static Deflater Deflater;
        public NibbleArray BlockLight;

        public byte[] Blocks;
        public NibbleArray Metadata;
        private int NonairCount;
        public NibbleArray SkyLight;
        public byte Y;

        public Section(byte Y)
        {
            this.Y = Y;
            Blocks = new byte[Width*Height*Depth];
            Metadata = new NibbleArray(Width*Height*Depth);
            BlockLight = new NibbleArray(Width*Height*Depth);
            SkyLight = new NibbleArray(Width*Height*Depth);
            for (int i = 0; i < SkyLight.Data.Length; i++)
                SkyLight.Data[i] = BlockLight.Data[i] = 0xFF;
            NonairCount = 0;
        }

        public bool IsAir
        {
            get { return NonairCount == 0; }
        }

        /// <summary>
        /// Sets the value of the block at the given position, relative to this section.
        /// </summary>
        public void SetBlock(Vector3 position, Block value)
        {
            var x = (int) position.X;
            var y = (int) position.Y;
            var z = (int) position.Z;
            int index = x + (z*Width) + (y*Height*Width);
            Blocks[index] = value.BlockID;
            Metadata[index] = value.Metadata;
            BlockLight[index] = value.BlockLight;
            SkyLight[index] = value.SkyLight;
            if (value is AirBlock)
                NonairCount--;
            else
                NonairCount++;
        }

        public Block GetBlock(Vector3 position)
        {
            var x = (int) position.X;
            var y = (int) position.Y;
            var z = (int) position.Z;
            int index = x + (z*Width) + (y*Height*Width);
            Block block = Blocks[index];
            block.Metadata = Metadata[index];
            block.SkyLight = SkyLight[index];
            block.BlockLight = BlockLight[index];
            return block;
        }
    }
}