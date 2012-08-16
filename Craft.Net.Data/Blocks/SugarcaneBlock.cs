namespace Craft.Net.Data.Blocks
{
    /// <summary>
    /// A Sugarcane block (ID = 83)
    /// </summary>
    /// <remarks></remarks>
    public class SugarcaneBlock : Block
    {
        /// <summary>
        /// The maximum height that a Sugarcane block can grow to (Default = 3)
        /// </summary>
        public static int MaxGrowth = 3;

        /// <summary>
        /// The Block ID for this block (83)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 83; }
        }

        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.Plant; }
        }
    }
}