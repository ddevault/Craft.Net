using System;

namespace Craft.Net.Server
{
    public class NibbleArray
    {
        public byte[] Data;
        public int Length
        {
            get
            {
                return Data.Length * 2;
            }
        }

        public NibbleArray(int Length)
        {
            Data = new byte[Length / 2];
        }

        public byte this[int index]
        {
            get
            {
                return (byte)(Data[index / 2] >> ((index + 1) % 2 * 4) & 0xF);
            }
            set
            {
                value &= 0xF;
                Data[index / 2] &= (byte)(0xF << (index % 2 * 4));
                Data[index / 2] |= (byte)(value << ((index + 1) % 2 * 4));
            }
        }
    }
}

