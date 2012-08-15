namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// An Iron block (ID = 42)
    /// </summary>
    /// <remarks></remarks>
    public class IronBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (42)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 42; }
        }
    }
}