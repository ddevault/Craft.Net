namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A block of ice (ID = 79)
    /// </summary>
    public class IceBlock : Block
    {
        /// <summary>
        /// This block's ID
        /// </summary>
        public override byte BlockID
        {
            get { return 79; }
        }

        /// <summary>
        /// Ice Block is a cube solid
        /// </summary>
        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.CubeSolid; }
        }
    }
}