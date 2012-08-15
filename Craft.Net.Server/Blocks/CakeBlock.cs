namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Cake Block (ID = 92)
    /// </summary>
    /// <remarks></remarks>
    public class CakeBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (92)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 92; }
        }

        /// <summary>
        /// Returns the opacity of a block.
        /// A Cake is a NonCubeSolid.
        /// </summary>
        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.NonCubeSolid; }
        }
    }
}