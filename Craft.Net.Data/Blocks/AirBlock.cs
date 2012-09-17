namespace Craft.Net.Data.Blocks
{
    public class AirBlock : Block
    {
        public override ushort Id
        {
            get { return 0; }
        }

        public override Size Size
        {
            get { return new Size(0, 0, 0); }
        }

        public override bool IsSolid
        {
            get { return false; }
        }
    }
}
