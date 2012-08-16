namespace Craft.Net.Data.Blocks
{
    /// <summary>
    /// An Active Repeater block (ID = 94)
    /// </summary>
    /// <remarks></remarks>
    public class RepeaterActiveBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (94)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 94; }
        }

        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.NonCubeSolid; }
        }
    }
}