namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Lever block (ID = 69)
    /// </summary>
    /// <remarks></remarks>
    public class LeverBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (69)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 69; }
        }

        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.NonSolidMechanism; }
        }
    }
}