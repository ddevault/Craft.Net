namespace Craft.Net.Server.Blocks
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