namespace Craft.Net.Data.Blocks
{
    public class LavaStillBlock : Block
    {
        public override byte BlockID
        {
            get { return 11; }
        }

        public override BlockOpacity Transparent
        {
            get { return BlockOpacity.Fluid; }
        }
    }
}