namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Grass block (ID = 2)
    /// </summary>
    /// <remarks></remarks>
    public class GrassBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (2)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 0x02; }
        }
    }
}