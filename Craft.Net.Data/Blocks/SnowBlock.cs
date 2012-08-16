namespace Craft.Net.Data.Blocks
{
    /// <summary>
    /// A full Snow block (ID = 80)
    /// </summary>
    /// <remarks></remarks>
    public class SnowBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (80)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 80; }
        }

        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.NonSolid; }
        }
    }
}