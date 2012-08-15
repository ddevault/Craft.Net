namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// Sapling ID=6
    /// </summary>
    public class SaplingBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (6)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 6; }
        }

        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.Plant; }
        }
    }
}