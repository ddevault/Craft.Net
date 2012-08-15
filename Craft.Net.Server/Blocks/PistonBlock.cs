namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Piston block (ID = 33)
    /// </summary>
    /// <remarks></remarks>
    public class PistonBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (33)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 33; }
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