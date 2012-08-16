namespace Craft.Net.Data
{
    /// <summary>
    /// Represents an array of 4-bit values.
    /// </summary>
    public class NibbleArray
    {
        /// <summary>
        /// The data in the nibble array. Each byte contains
        /// two nibbles, stored in big-endian.
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// Creates a new nibble array with the given number of nibbles.
        /// </summary>
        public NibbleArray(int length)
        {
            Data = new byte[length/2];
        }

        /// <summary>
        /// Gets the current number of nibbles in this array.
        /// </summary>
        public int Length
        {
            get { return Data.Length * 2; }
        }

        /// <summary>
        /// Gets or sets a nibble at the given index.
        /// </summary>
        public byte this[int index]
        {
            get { return (byte)(Data[index/2] >> ((index + 1)%2*4) & 0xF); }
            set
            {
                value &= 0xF;
                Data[index/2] &= (byte)(0xF << ((index + 1) % 2 * 4));
                Data[index/2] |= (byte)(value << (index % 2 * 4));
            }
        }
    }
}