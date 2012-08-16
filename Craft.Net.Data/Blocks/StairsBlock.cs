namespace Craft.Net.Data.Blocks
{
    /// <summary>
    /// The base class for Stair blocks
    /// </summary>
    /// <remarks></remarks>
    public abstract class StairsBlock : Block
    {
        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.NonCubeSolid; }
        }
    }
}