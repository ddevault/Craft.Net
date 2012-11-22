namespace Craft.Net.Data.Items
{
    public class SaddleItem : Item
    {
        public override ushort Id
        {
            get
            {
                return 329;
            }
        }

        public override byte MaximumStack
        {
            get { return 1; }
        }
    }
}