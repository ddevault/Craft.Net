namespace Craft.Net.Data.Blocks
{
    /// <summary>
    /// A Piston Plunger block (ID = 33)
    /// This block is the block on the end of a piston which moves forward and backwards
    /// </summary>
    /// <remarks></remarks>
    public class PistonPlungerBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (33)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 33; }
        }

        public override byte Metadata
        {
            get { return base.Metadata; }
            set { base.Metadata = value; }
        }
    }
}