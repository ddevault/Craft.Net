namespace Craft.Net.Data.Items
{
    
    public class MinecartItem : Item
    {
        public override ushort Id
        {
            get
            {
                return 328;
            }
        }

        public override byte MaximumStack
        {
            get { return 1; }
        }
    }
}
