namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Rose block (ID = 38)
    /// </summary>
    /// <remarks></remarks>
    public class RoseBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (38)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 38; }
        }

        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.Plant; }
        }
    }
}