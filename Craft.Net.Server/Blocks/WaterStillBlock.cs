namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Still Water block (ID = 9)
    /// </summary>
    /// <remarks></remarks>
    public class WaterStillBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (9)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 9; }
        }

        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.Fluid; }
        }
    }
}