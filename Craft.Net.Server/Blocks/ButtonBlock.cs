namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Button Block (ID = 77)
    /// </summary>
    /// <remarks></remarks>
    public class ButtonBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (77)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 77; }
        }


        /// <summary>
        /// Returns the opacity of a block.
        /// A Button Block is a NonSolidMechanism
        /// </summary>
        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.NonSolidMechanism; }
        }
    }
}