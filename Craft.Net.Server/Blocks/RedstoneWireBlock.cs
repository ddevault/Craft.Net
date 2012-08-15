namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Redstone Wire block (ID = 55)
    /// </summary>
    /// <remarks></remarks>
    public class RedstoneWireBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (55)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 55; }
        }

        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.NonSolidMechanism; }
        }
    }
}