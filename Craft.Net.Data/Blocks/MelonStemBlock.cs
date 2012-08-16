namespace Craft.Net.Data.Blocks
{
    /// <summary>
    /// Pumkpin Stem ID=106
    /// </summary>
    public class MelonStemBlock : Block
    {
        public override byte BlockID
        {
            get { return 105; }
        }

        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.Plant; }
        }
    }
}