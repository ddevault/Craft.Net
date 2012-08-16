namespace Craft.Net.Data.Blocks
{
    /// <summary>
    /// A Seed block (ID = 59)
    /// </summary>
    /// <remarks></remarks>
    public class SeedBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (59)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 59; }
        }

        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.Plant; }
        }
    }
}