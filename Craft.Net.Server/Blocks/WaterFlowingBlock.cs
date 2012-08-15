namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Flowing Water block (ID = 8)
    /// </summary>
    /// <remarks></remarks>
    public class WaterFlowingBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (8)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 8; }
        }

        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.Fluid; }
        }
    }
}