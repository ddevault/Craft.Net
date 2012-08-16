namespace Craft.Net.Data.Blocks
{
    /// <summary>
    /// A Sticky Piston block (ID = 29)
    /// </summary>
    /// <remarks></remarks>
    public class StickyPistonBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (29)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 29; }
        }

        /// <summary>
        /// Pistons are cube solids.
        /// NOTE: THIS IS ONLY WHEN RETRACTED.
        /// </summary>
        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.CubeSolid; }
        }
    }
}