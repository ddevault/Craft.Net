namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Dandelion Block (ID=37)
    /// </summary>
    public class DandelionBlock : Block
    {
        /// <summary>
        /// Returns a block's ID.
        /// Dandelion (ID=37)
        /// </summary>
        public override byte BlockID
        {
            get { return 37; }
        }

        /// <summary>
        /// Returns a blocks transparency.
        /// A Dandelion is a plant.
        /// </summary>
        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.Plant; }
        }
    }
}