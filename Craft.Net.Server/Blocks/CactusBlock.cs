namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Cactus Block (ID = 81)
    /// </summary>
    /// <remarks></remarks>
    public class CactusBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (81)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 81; }
        }


        /// <summary>
        /// Returns the opacity of a block.
        /// A Cactus Block is a plant.
        /// </summary>
        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.Plant; }
        }
    }
}