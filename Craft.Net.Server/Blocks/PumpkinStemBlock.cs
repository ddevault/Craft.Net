namespace Craft.Net.Server.Blocks
{
    /// <summary>
    /// Pumpkin Stem ID=106
    /// </summary>
    public class PumpkinStemBlock : Block
    {
        public override byte BlockID
        {
            get { return 104; }
        }

        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.Plant; }
        }
    }
}