namespace Craft.Net.Data.Blocks
{
    /// <summary>
    /// FireBlock (ID = 51)
    /// </summary>
    public class FireBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (51)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 51; }
        }

        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.NonSolid; }
        }
    }
}