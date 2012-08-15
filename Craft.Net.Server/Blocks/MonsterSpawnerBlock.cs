namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// A Monster Spawner block (ID = 52)
    /// </summary>
    /// <remarks></remarks>
    public class MonsterSpawnerBlock : Block
    {
        /// <summary>
        /// The Block ID for this block (52)
        /// </summary>
        /// <remarks></remarks>
        public override byte BlockID
        {
            get { return 52; }
        }


        /// <summary>
        /// Monster spawner is a cube solid.
        /// </summary>
        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.CubeSolid; }
        }
    }
}