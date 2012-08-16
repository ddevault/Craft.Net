namespace Craft.Net.Data.Blocks
{
    public enum WoolColor
    {
        White = 0,
        Orange = 1,
        Magenta = 2,
        LightBlue = 3,
        Yellow = 4,
        Lime = 5,
        Pink = 6,
        Grey = 7,
        LightGrey = 8,
        Cyan = 9,
        Purple = 10,
        Blue = 11,
        Brown = 12,
        Green = 13,
        Red = 14,
        Black = 15
    }

    /// <summary>
    /// A Wool Block (ID = 35)
    /// </summary>
    /// <remarks></remarks>
    public class WoolBlock : Block
    {
        public WoolBlock()
        {
        }

        public WoolBlock(WoolColor color)
        {
            Metadata = (byte)color;
        }

        /// <summary>
        /// The Block ID for this block (35)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 35; }
        }
    }
}