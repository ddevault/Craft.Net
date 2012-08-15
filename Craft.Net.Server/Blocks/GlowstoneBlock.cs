namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Glowstone block (ID = 89)
    /// </summary>
    /// <remarks></remarks>
    public class GlowstoneBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (89)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 89; }
        }


        /// <summary>
        /// Glowstone is a cube solid.
        /// </summary>
        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.CubeSolid; }
        }
    }
}