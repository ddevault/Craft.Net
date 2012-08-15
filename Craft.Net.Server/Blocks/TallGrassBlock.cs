namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Tall Grass block (ID 31)
    /// </summary>
    /// <remarks></remarks>
    public class TallGrassBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (31)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 0x1F; }
        }

        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.Plant; }
        }
    }
}