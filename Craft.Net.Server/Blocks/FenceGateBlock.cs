namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// Fence Gate ID=107
    /// </summary>
    public class FenceGateBlock : Block
    {
        /// <summary>
        /// Returns the block ID of the block.
        /// Fence Gate ID=107
        /// </summary>
        public override byte BlockID
        {
            get { return 107; }
        }

        /// <summary>
        /// Returns the opacity of a block.
        /// Fence Gate is a NonCubeSolid.
        /// </summary>
        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.NonCubeSolid; }
        }
    }
}