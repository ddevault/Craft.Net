namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// Fence Block ID = 85
    /// </summary>
    /// <remarks></remarks>
    public class FenceBlock : Block
    {
        /// <summary>
        /// The Block ID for this block
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 85; }
        }

        /// <summary>
        /// Returns the opacity of a block.
        /// Fence Block is a NonCubeSolid.
        /// </summary>
        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.NonCubeSolid; }
        }
    }
}