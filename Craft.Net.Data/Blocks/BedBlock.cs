namespace Craft.Net.Data.Blocks
{
    /// <summary>
    /// A Bed block (ID = 26)
    /// </summary>
    public class BedBlock : Block
    {
        /// <summary>
        /// This block's ID
        /// </summary>
        public override byte BlockID
        {
            get { return 26; }
        }

        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.NonCubeSolid; }
        }
    }
}