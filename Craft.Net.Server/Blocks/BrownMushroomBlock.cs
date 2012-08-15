namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Brown Mushroom Block (ID = 39)
    /// </summary>
    /// <remarks></remarks>
    public class BrownMushroomBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (39)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 39; }
        }

        /// <summary>
        /// Returns the opacity of a block.
        /// A Brown Mushroom is a plant.
        /// </summary>
        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.Plant; }
        }
    }
}